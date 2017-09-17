using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SourceDemoParser.Net;

namespace SourceDemoParser.Net.Extensions
{
	// Todo: demo simulation for each tick
	public static class SourceExtensions
	{
		private static Dictionary<Type, object> _instanceCache = new Dictionary<Type, object>();
		private static List<AdjustmentCache> _adjustmentCache = new List<AdjustmentCache>();
		
		// Header
		public static int GetTickrate(this SourceDemo demo)
			=> (int)Math.Round(demo.PlaybackTicks / demo.PlaybackTime);
		public static float GetTicksPerSecond(this SourceDemo demo)
			=> demo.PlaybackTime / demo.PlaybackTicks;
		// Data
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, DemoMessageType type)
			=> demo.Messages.Where(message => message.Type == type).ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByTick(this SourceDemo demo, int tick)
			=> demo.Messages.Where(message => message.CurrentTick == tick).ToList();
		public static IDemoMessage FindMessage(this SourceDemo demo, string command)
			=> demo.GetMessagesByType(DemoMessageType.ConsoleCmd).FirstOrDefault(message => (message.Frame as ConsoleCmdFrame).ConsoleCommand == command);
		public static IDemoMessage FindMessage(this SourceDemo demo, Vector3f position)
			=> demo.GetMessagesByType(DemoMessageType.Packet).FirstOrDefault(message => Vector3f.Equals((message.Frame as PacketFrame).PlayerPosition, position));
		// Adjustments
		public static async Task<SourceDemo> AdjustAsync(this SourceDemo demo, Assembly asm = null)
		{
			await LoadAndCacheDemos(asm ?? Assembly.GetExecutingAssembly()).ConfigureAwait(false);
			var tickrate = demo.GetTickrate();
			var candidates = new List<AdjustmentCache>();
			foreach (var cache in _adjustmentCache)
			{
				// Match game
				if (cache.GameDirectory != demo.GameDirectory)
					continue;
				
				// Match tickrate
				if ((int)cache.DefaultTickrate != tickrate)
					continue;
				
				candidates.Add(cache);
			}
			
			// Adjustment logic:
			// 1.a) Adjustments with mapname will be parsed first
			// 1.b) Adjustments without mapname will be parsed second (optionally)
			
			if (candidates.Count > 0)
			{
				var packets = demo.GetMessagesByType(DemoMessageType.Packet);
				var cmds = demo.GetMessagesByType(DemoMessageType.ConsoleCmd);
				
				var adjustments = new List<Adjustment>();
				foreach (var candidate in candidates)
					foreach (var adjustment in candidate.Adjustments)
						adjustments.Add(adjustment);
				
				// Adjust start
				var starttick = default(int?);
				var starts = adjustments.Where(a => a.Type == AdjustmentType.Start && a.MapName == demo.MapName);
				var iscommon = default(bool);
				
				if (!starts.Any())
				{
					iscommon = true;
					starts = adjustments.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = GetTick(starts, packets, cmds);
				}
				else
					starttick = GetTick(starts, packets, cmds);
				
				if ((starttick == null) && (!iscommon))
				{
					starts = adjustments.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = GetTick(starts, packets, cmds);
				}
				
				// Adjust ending
				var endtick = default(int?);
				var ends = adjustments.Where(a => a.Type == AdjustmentType.End && a.MapName == demo.MapName);
				iscommon = default(bool);
				
				if (!ends.Any())
				{
					iscommon = true;
					ends = adjustments.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = GetTick(ends, packets, cmds);
				}
				else
					endtick = GetTick(ends, packets, cmds);

				if ((endtick == null) && (!iscommon))
				{
					ends = adjustments.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = GetTick(ends, packets, cmds);
				}
				
				// Final adjustment logic
				if (starttick != null)
				{
					if (endtick != null)
						return await demo.AdjustExact(startTick: (int)starttick, endTick: (int)endtick);
					return await demo.AdjustExact(startTick: (int)starttick);
				}
				else if (endtick != null)
					return await demo.AdjustExact(endTick: (int)endtick);
			}
			return default(SourceDemo);
		}
		internal static object GetInstance(Type t)
		{
			var instance = default(object);
			if (_instanceCache.TryGetValue(t, out instance))
				return instance;
			throw new Exception($"Could not load instance cache of {t}");
		}
		internal static int? GetTick(IEnumerable<Adjustment> adjustments, IEnumerable<IDemoMessage> packets, IEnumerable<IDemoMessage> cmds)
		{
			var current = adjustments.First().Root;
			var instance = GetInstance(current);
			foreach (var adjustment in adjustments)
			{
				if (adjustment.Root != current)
					instance = GetInstance(current);
				
				var messages = default(IEnumerable<IDemoMessage>);
				if (adjustment.Parameter == PlayerStructType.Position)
					messages = packets;
				else if (adjustment.Parameter == PlayerStructType.Command)
					messages = cmds;
				
				adjustment.Result = GetResult(messages, instance, adjustment);
			}
			
			// Multiple matches logic:
			// 1.a) For multiple start adjustments, the higest tick will be taken
			// 1.b) For multiple start adjustments, the lowest offset tick will be taken
			// 2.a) For multiple end adjustments, the lowest tick will be taken
			// 2.b) For multiple end adjustments, the highest offset tick will be taken
			
			var results = adjustments.Where(s => s.Result.Found == true);
			var count = results.Count();
			if (count > 0)
			{
				var isstart = results.First().Type == AdjustmentType.Start;
				
				// Some redundancy here but it's fast
				if (count == 1)
					return results.First().Result.FoundTickAt + results.First().Offset;
				
				var match = (isstart) ? results.Max(r => r.Result.FoundTickAt)
						      : results.Min(r => r.Result.FoundTickAt);
				results = results.Where(r => r.Result.FoundTickAt == match);
				count = results.Count();
				if (count == 1)
					return results.First().Result.FoundTickAt + results.First().Offset;
				
				match = (isstart) ? results.Min(r => r.Offset)
						  : results.Max(r => r.Offset);
				results = results.Where(r => r.Offset == match);
				count = results.Count();
				if (count == 1)
					return results.First().Result.FoundTickAt + results.First().Offset;
				
				throw new Exception("Multiple adjustment matches (with same tick offset too)!");
			}
			return null;
		}
		internal static AdjustmentResult GetResult(IEnumerable<IDemoMessage> messages, object instance, Adjustment adjustment)
		{
			var cpos = default(Vector3f);
			var ccmd = default(string);
			foreach (var message in messages)
			{
				var parameter = default(object);
				if (adjustment.Parameter == PlayerStructType.Position)
				{
					parameter = new PlayerPosition
					{
						Old = cpos,
						Current = (cpos = ((PacketFrame)message.Frame).PlayerPosition)
					};
				}
				else if (adjustment.Parameter == PlayerStructType.Command)
				{
					parameter = new PlayerCommand
					{
						Old = ccmd,
						Current = (ccmd = ((ConsoleCmdFrame)message.Frame).ConsoleCommand)
					};
				}
				
				// Invoke
				var result = default(bool);
				try
				{
					result = (bool)adjustment.Method.Invoke(instance, new object[1] { parameter });
				}
				catch (Exception e)
				{
					throw new Exception($"[{message.CurrentTick}] Exception occured when invoking method: {adjustment.Method.Name}\n" + e.ToString());
				}
				
				if (!result)
					continue;
				
				return new AdjustmentResult
				{
					Found = true,
					FoundTickAt = message.CurrentTick
				};
			}
			return new AdjustmentResult();
		}
		public static async Task<SourceDemo> AdjustAsync<T>(this SourceDemo demo)
			where T : class, ISourceDemo, new()
		{
			var type = typeof(T);
			if (!(IsValidClass(type)))
				throw new InvalidOperationException("Class cannot be used for parsing!");
			
			//var instance = new T();
			//var adjustments = ConvertToAdjustment(type.GetMethods());
			//_cache.Add(instance, adjustments.ToList());
			// Todo
			
			return default(SourceDemo);
		}
		public static async Task<SourceDemo> AdjustFlagAsync(this SourceDemo demo, string saveFlag = "echo #SAVE#")
		{
			if (demo.Messages.Count == 0)
				throw new InvalidOperationException("Cannot adjust ticks without parsed messages!");
			var tps = demo.GetTicksPerSecond();
			var flag = demo.FindMessage(saveFlag);
			if (flag != null)
			{
				await demo.AdjustExact(flag.CurrentTick).ConfigureAwait(false);
				return demo;
			}
			return default(SourceDemo);
		}
		public static Task<SourceDemo> AdjustExact(this SourceDemo demo, int endTick = 0, int startTick = 0)
		{
			if (endTick < 1)
				endTick = demo.Messages.Where(m => m.CurrentTick > 0).Last().CurrentTick;
			var delta = endTick - startTick;
			if (delta < 0)
				throw new Exception("Start tick is greater than end tick!");
			var tps = demo.GetTicksPerSecond();
			demo.PlaybackTicks = delta;
			demo.PlaybackTime = tps * delta;
			return Task.FromResult(demo);
		}
		// Internal stuff
		internal static Task<int> LoadAndCacheDemos(Assembly asm)
		{
			// Load all classes who implemented ISourceDemo
			foreach (var type in asm.GetTypes())
			{
				if (!IsValidClass(type))
					continue;
				
				if (_instanceCache.ContainsKey(type))
					continue;
				
				var instance = Activator.CreateInstance(type, null);
				if (instance == null)
					throw new Exception($"Failed to create instance of type: {type}");
				
				var demo = instance as ISourceDemo;
				if (demo == null)
					throw new Exception($"Instance does not implement ISourceDemo.");
				
				// Cache stuff
				_instanceCache.Add(type, instance);
				_adjustmentCache.Add(new AdjustmentCache
				{
					Demo = demo,
					Adjustments = ConvertToAdjustment(type).ToList()
				});
			}
			return Task.FromResult(_adjustmentCache.Count());
		}
		internal static IEnumerable<Adjustment> ConvertToAdjustment(Type t)
		{
			foreach (var method in t.GetMethods())
			{
				if (!IsValidMethod(method))
					continue;
				
				var builder = new AdjustmentBuilder(method);
				yield return builder.Build(t);
			}
			yield break;
		}
		internal static bool IsValidClass(Type type)
		{
			// Check interface
			if (type.GetInterfaces().Contains(typeof(ISourceDemo)))
			{
				// Check info
				var info = type.GetTypeInfo();
				if ((info.IsClass) && (info.IsPublic) && !(info.IsAbstract))
				{
					// Check methods
					var methods = info.GetMethods().Where(m => IsValidMethod(m));
					if (methods.Count() >= 1)
						return true;
				}
			}
			return false;
		}
		internal static bool IsValidMethod(MethodInfo method)
		{
			// public bool
			if ((method.IsPublic) && (method.ReturnType == typeof(bool)) && !(method.IsStatic))
			{
				// (PlayerPosition ...) or (PlayerCommand ...)
				var parameters = method.GetParameters();
				if (parameters.Count() == 1)
				{
					var parameter = parameters.First();
					if ((parameter.ParameterType == typeof(PlayerPosition))
					|| (parameter.ParameterType == typeof(PlayerCommand)))
					{
						// [StartAdjustment] or [EndAdjustment]
						var attributes = method.GetCustomAttributes()
						.Where(attribute =>
						{
							var type = attribute.GetType();
							if ((type == typeof(StartAdjustmentAttribute))
							|| (type == typeof(EndAdjustmentAttribute)))
								return true;
							return false;
						});
						if (attributes.Count() == 1)
							return true;
					}
				}
			}
			return false;
		}
	}
}