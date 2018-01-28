using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	public static class SourceExtensions
	{
		private static readonly Dictionary<TypeInfo, object> _instanceCache = new Dictionary<TypeInfo, object>();
		private static readonly List<AdjustmentCache> _adjustmentCache = new List<AdjustmentCache>();

		// Header
		public static int GetTickrate(this SourceDemo demo)
			=> (int)Math.Round(demo.PlaybackTicks / demo.PlaybackTime);
		public static float GetTicksPerSecond(this SourceDemo demo)
			=> demo.PlaybackTime / demo.PlaybackTicks;

		// Data
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, DemoMessageType type)
			=> demo.Messages.Where(message => message.Type.Name == type.Name).ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, string typeName)
			=> demo.Messages.Where(message => message.Type.Name == typeName).ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByTick(this SourceDemo demo, int tick)
			=> demo.Messages.Where(message => message.CurrentTick == tick).ToList();

		public static IDemoMessage FindMessage(this SourceDemo demo, string command)
			=> demo.GetMessagesByType("ConsoleCmd").FirstOrDefault(message => (message.Frame as ConsoleCmdFrame).ConsoleCommand == command);
		public static IDemoMessage FindMessage(this SourceDemo demo, Vector position)
			=> demo.GetMessagesByType("Packet").FirstOrDefault(message => Vector.Equals((message.Frame as PacketFrame).Infos[0].ViewOrigin, position));

		// Extra
		public static void ParseFrames(this SourceDemo demo)
			=> demo.Messages.ForEach(async m => await m.Frame.ParseData(demo).ConfigureAwait(false));

		// Adjustments
		public static Task<SourceDemo> AdjustExact(this SourceDemo demo, int endTick = 0, int startTick = 0)
		{
			if (endTick < 1)
				endTick = demo.Messages.Last(m => m.CurrentTick > 0).CurrentTick;

			var delta = endTick - startTick;
			if (delta < 0)
				throw new Exception("Start tick is greater than end tick.");

			var tps = demo.GetTicksPerSecond();
			demo.PlaybackTicks = delta;
			demo.PlaybackTime = tps * delta;
			return Task.FromResult(demo);
		}
		public static async Task<SourceDemo> AdjustFlagAsync(this SourceDemo demo, string saveFlag = "echo #SAVE#")
		{
			if (demo.Messages.Count == 0)
				throw new InvalidOperationException("Cannot adjust ticks without parsed messages.");

			var flag = demo.FindMessage(saveFlag);
			if (flag != null)
			{
				await demo.AdjustExact(flag.CurrentTick).ConfigureAwait(false);
				return demo;
			}
			return demo;
		}

		// Custom Adjustments
		public static async Task<SourceDemo> AdjustAsync(this SourceDemo demo)
		{
			if (_adjustmentCache.Count == 0)
				throw new Exception("No adjustments have been loaded.");

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
				var packets = demo.GetMessagesByType("Packets");
				var cmds = demo.GetMessagesByType("ConsoleCmd");

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
						starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
				}
				else
					starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);

				if ((starttick == null) && (!iscommon))
				{
					starts = adjustments.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
				}

				// Adjust ending
				var endtick = default(int?);
				var ends = adjustments.Where(a => a.Type == AdjustmentType.End && a.MapName == demo.MapName);
				iscommon = false;

				if (!ends.Any())
				{
					iscommon = true;
					ends = adjustments.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
				}
				else
					endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);

				if ((endtick == null) && (!iscommon))
				{
					ends = adjustments.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
				}

				// Final adjustment logic
				if (starttick != null)
				{
					if (endtick != null)
						return await demo.AdjustExact(startTick: (int)starttick, endTick: (int)endtick).ConfigureAwait(false);
					return await demo.AdjustExact(startTick: (int)starttick).ConfigureAwait(false);
				}
				else if (endtick != null)
					return await demo.AdjustExact(endTick: (int)endtick).ConfigureAwait(false);
			}
			return demo;
		}
		// Returns true if new adjustments have been loaded
		public static async Task<bool> DiscoverAsync(Assembly asm = null)
		{
			asm = asm ?? typeof(SourceExtensions).GetTypeInfo().Assembly;
			var loaded = 0u;
			foreach (var type in asm.DefinedTypes)
			{
				if (!IsValidClass(type))
					continue;
				if (_instanceCache.ContainsKey(type))
					continue;

				await AddToCache(type).ConfigureAwait(false);
				++loaded;
			}
			return loaded != 0u;
		}
		// Returns true on success
		public static async Task<bool> LoadAsync<T>()
		{
			var type = typeof(T).GetTypeInfo();
			if (_instanceCache.ContainsKey(type))
				return false;

			if (!IsValidClass(type))
				return false;

			await AddToCache(type).ConfigureAwait(false);
			return true;
		}
		// Internal stuff
		internal static Task<object> GetInstance(TypeInfo t)
		{
			if (_instanceCache.TryGetValue(t, out object instance))
				return Task.FromResult(instance);
			throw new Exception($"Could not load instance cache of {t}.");
		}
		internal static async Task<int?> GetTickAsync(IEnumerable<Adjustment> adjustments, IEnumerable<IDemoMessage> packets, IEnumerable<IDemoMessage> cmds)
		{
			var current = adjustments.First().Root;
			var instance = await GetInstance(current).ConfigureAwait(false);
			foreach (var adjustment in adjustments)
			{
				if (adjustment.Root != current)
					instance = await GetInstance(current).ConfigureAwait(false);

				var messages = default(IEnumerable<IDemoMessage>);
				if (adjustment.Parameter == PlayerStructType.Position)
					messages = packets;
				else if (adjustment.Parameter == PlayerStructType.Command)
					messages = cmds;

				adjustment.Result = await GetResult(messages, instance, adjustment).ConfigureAwait(false);
			}

			// Multiple matches logic:
			// 1.a) For multiple start adjustments, the higest tick will be taken
			// 1.b) For multiple start adjustments, the lowest offset tick will be taken
			// 2.a) For multiple end adjustments, the lowest tick will be taken
			// 2.b) For multiple end adjustments, the highest offset tick will be taken

			var results = adjustments.Where(s => s.Result.Found);
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

				throw new Exception($"Multiple adjustment matches: {string.Join("\n", results.Select(r => r.Method.Name))}.");
			}
			return null;
		}
		internal static Task<AdjustmentResult> GetResult(IEnumerable<IDemoMessage> messages, object instance, Adjustment adjustment)
		{
			var cpos = default(Vector);
			var ccmd = default(string);
			foreach (var message in messages)
			{
				var parameter = default(object);
				if (adjustment.Parameter == PlayerStructType.Position)
				{
					parameter = new PlayerPosition
					{
						Old = cpos,
						Current = (cpos = ((PacketFrame)message.Frame).Infos[0].ViewOrigin)
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
					throw new Exception($"[{message.CurrentTick}] Exception occured when invoking method: {adjustment.Method.Name}.\n{e}");
				}

				if (!result)
					continue;

				return Task.FromResult(new AdjustmentResult
				{
					Found = true,
					FoundTickAt = message.CurrentTick
				});
			}
			return Task.FromResult(new AdjustmentResult());
		}
		internal static Task AddToCache(TypeInfo iDemo)
		{
			// Note: ctor has to be public
			var instance = Activator.CreateInstance(iDemo.AsType(), null);
			if (instance == null)
				throw new Exception($"Failed to create instance of type: {iDemo}.");

			var demo = instance as ISourceDemo;
			if (demo == null)
				throw new Exception($"Instance does not implement ISourceDemo.");

			// Cache stuff
			_instanceCache.Add(iDemo, instance);
			_adjustmentCache.Add(new AdjustmentCache
			{
				Demo = demo,
				Adjustments = ConvertToAdjustment(iDemo).ToList()
			});
			return Task.CompletedTask;
		}
		internal static IEnumerable<Adjustment> ConvertToAdjustment(TypeInfo iDemo)
		{
			foreach (var method in iDemo.DeclaredMethods)
			{
				if (!IsValidMethod(method))
					continue;

				var builder = new AdjustmentBuilder(method);
				yield return builder.Build(iDemo);
			}
		}
		internal static bool IsValidClass(TypeInfo info)
		{
			// Check interface
			if ((typeof(ISourceDemo).GetTypeInfo().IsAssignableFrom(info)) && (info.IsClass) && (info.IsPublic) && !(info.IsAbstract))
			{
				// Check methods
				var methods = info.DeclaredMethods.Where(m => IsValidMethod(m));
				if (methods.Any())
					return true;
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
				if (parameters.Length == 1)
				{
					var parameter = parameters[0].ParameterType;
					if ((parameter == typeof(PlayerPosition)) || (parameter == typeof(PlayerCommand)))
					{
						// [StartAdjustment] or [EndAdjustment]
						var attributes = method.GetCustomAttributes()
							.Where(attribute => (attribute is StartAdjustmentAttribute) || (attribute is EndAdjustmentAttribute));
						if (attributes.Count() == 1)
							return true;
					}
				}
			}
			return false;
		}
	}
}