using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	public static class Adjustments
	{
		private static readonly Dictionary<TypeInfo, AdjustmentCache> _adjustmentsCache;

		static Adjustments() => _adjustmentsCache = new Dictionary<TypeInfo, AdjustmentCache>();

		public static async Task<bool> DiscoverAsync(Assembly asm = null)
		{
			asm = asm ?? typeof(Adjustments).GetTypeInfo().Assembly;
			var loaded = 0;
			foreach (var type in asm.DefinedTypes)
			{
				if (_adjustmentsCache.ContainsKey(type))
					continue;
				if (await Task.Run(async () => !await IsValidClassAsync(type)).ConfigureAwait(false))
					continue;

				await AddToCacheAsync(type).ConfigureAwait(false);
				loaded++;
			}
			return loaded != 0;
		}
		public static async Task<bool> LoadAsync<T>()
		{
			var type = typeof(T).GetTypeInfo();
			if (_adjustmentsCache.ContainsKey(type))
				return false;

			if (await Task.Run(async () => !await IsValidClassAsync(type)).ConfigureAwait(false))
				return false;

			await AddToCacheAsync(type).ConfigureAwait(false);
			return true;
		}
		public static Task<bool> Remove<T>()
		{
			var type = typeof(T).GetTypeInfo();
			if (!_adjustmentsCache.ContainsKey(type))
				return Task.FromResult(false);
			
			return Task.FromResult(_adjustmentsCache.Remove(type));
		}
		public static Task<bool> RemoveAll(Assembly asm = null)
		{
			asm = asm ?? typeof(Adjustments).GetTypeInfo().Assembly;
			var removed = 0;
			foreach (var type in asm.DefinedTypes)
			{
				if (!_adjustmentsCache.ContainsKey(type))
					continue;
				
				if (_adjustmentsCache.Remove(type))
					removed++;
			}
			return Task.FromResult(removed != 0);
		}
		public static async Task<SourceDemo> AdjustAsync(this SourceDemo demo)
		{
			if (_adjustmentsCache.Count == 0)
				throw new Exception("No adjustments have been loaded.");

			var tickrate = demo.GetTickrate();
			var candidates = new List<AdjustmentCache>();
			foreach (var cache in _adjustmentsCache.Select(c => c.Value))
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
				var packets = demo.GetMessagesByType(new Types.Packet());
				var cmds = demo.GetMessagesByType(new Types.ConsoleCmd());

				var adjustments = new List<Adjustment>();
				foreach (var candidate in candidates)
					foreach (var adjustment in candidate.Adjustments)
						adjustments.Add(adjustment);

				// Adjust start
				var starttick = default(int?);
				var starts = adjustments
					.Where(a => a.Type == AdjustmentType.Start && a.MapName == demo.MapName);
				var iscommon = default(bool);

				if (!starts.Any())
				{
					iscommon = true;
					starts = adjustments
						.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
				}
				else
					starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);

				if ((starttick == null) && (!iscommon))
				{
					starts = adjustments
						.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = await GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
				}

				// Adjust ending
				var endtick = default(int?);
				var ends = adjustments
					.Where(a => a.Type == AdjustmentType.End && a.MapName == demo.MapName);
				iscommon = false;

				if (!ends.Any())
				{
					iscommon = true;
					ends = adjustments
						.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
				}
				else
					endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);

				if ((endtick == null) && (!iscommon))
				{
					ends = adjustments
						.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = await GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
				}

				// Final adjustment logic
				if (starttick != null)
				{
					if (endtick != null)
						return await demo.AdjustExact(startTick: (int)starttick, endTick: (int)endtick)
							.ConfigureAwait(false);
					return await demo.AdjustExact(startTick: (int)starttick).ConfigureAwait(false);
				}
				else if (endtick != null)
					return await demo.AdjustExact(endTick: (int)endtick).ConfigureAwait(false);
			}
			return demo;
		}

		// Internal stuff
		internal static Task<object> GetInstance(TypeInfo t)
		{
			if (_adjustmentsCache.TryGetValue(t, out var cache))
				return Task.FromResult(cache.Instance);
			throw new Exception($"Could not load instance cache of {t}.");
		}
		internal static async Task AddToCacheAsync(TypeInfo id)
		{
			// Note: ctor has to be public
			var instance = Activator.CreateInstance(id.AsType(), null);
			if (instance == null)
				throw new Exception($"Failed to create instance of type: {id}.");

			var demo = instance as ISourceDemo;
			if (demo == null)
				throw new Exception("Instance does not implement ISourceDemo.");

			// Cache stuff
			var adjustments = await GetAdjustmentAsync(id).ConfigureAwait(false);
			_adjustmentsCache.Add(id, new AdjustmentCache()
			{
				Instance = instance,
				Demo = demo,
				Adjustments = adjustments
			});
		}
		internal static async Task<int?> GetTickAsync(
			IEnumerable<Adjustment> adjustments,
			IEnumerable<IDemoMessage> packets,
			IEnumerable<IDemoMessage> cmds)
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

				adjustment.Result = await GetResultAsync(messages, instance, adjustment).ConfigureAwait(false);
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

				var match = (isstart)
					? results.Max(r => r.Result.FoundTickAt)
					: results.Min(r => r.Result.FoundTickAt);
				results = results.Where(r => r.Result.FoundTickAt == match);
				count = results.Count();
				if (count == 1)
					return results.First().Result.FoundTickAt + results.First().Offset;

				match = (isstart)
					? results.Min(r => r.Offset)
					: results.Max(r => r.Offset);
				results = results.Where(r => r.Offset == match);
				count = results.Count();
				if (count == 1)
					return results.First().Result.FoundTickAt + results.First().Offset;

				throw new Exception("Multiple adjustment matches: " +
					$"{string.Join("\n", results.Select(r => r.Method.Name))}.");
			}
			return null;
		}
		internal static async Task<AdjustmentResult> GetResultAsync(
			IEnumerable<IDemoMessage> messages,
			object instance,
			Adjustment adjustment)
		{
			var cpos = default(Vector);
			var ccmd = default(string);
			foreach (var message in messages)
			{
				var parameter = default(object);
				if (adjustment.Parameter == PlayerStructType.Position)
				{
					parameter = new PlayerPosition()
					{
						Previous = cpos,
						Current = (cpos = ((PacketFrame)message.Frame).Infos[0].ViewOrigin)
					};
				}
				else if (adjustment.Parameter == PlayerStructType.Command)
				{
					parameter = new PlayerCommand()
					{
						Previous = ccmd,
						Current = (ccmd = ((ConsoleCmdFrame)message.Frame).ConsoleCommand)
					};
				}

				// Invoke
				var result = default(bool);
				try
				{
					result = await Task.Run(() => (bool)adjustment.Method
						.Invoke(instance, new object[1] { parameter })).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					throw new Exception($"[{message.Tick}] Exception occured when invoking method: " +
						$"{adjustment.Method.Name}.\n{e}");
				}

				if (!result)
					continue;

				return new AdjustmentResult()
				{
					Found = true,
					FoundTickAt = message.Tick
				};
			}
			return new AdjustmentResult();
		}
		internal static async Task<IEnumerable<Adjustment>> GetAdjustmentAsync(TypeInfo id)
		{
			var adjustments = new List<Adjustment>();
			foreach (var method in id.DeclaredMethods)
			{
				if (await Task.Run(() => !IsValidMethod(method)).ConfigureAwait(false))
					continue;

				var adjustment = new AdjustmentBuilder()
					.WithId(id)
					.WithMethod(method);

				adjustments.Add(await Task.Run(() => adjustment.Build()).ConfigureAwait(false));
			}
			return adjustments;
		}
		internal static async Task<bool> IsValidClassAsync(TypeInfo info)
		{
			// Check interface
			if ((typeof(ISourceDemo).GetTypeInfo().IsAssignableFrom(info))
				&& (info.IsClass) && (info.IsPublic) && !(info.IsAbstract))
			{
				// Check methods
				var methods = new List<MethodInfo>();
				foreach (var method in info.DeclaredMethods)
				{
					if (await Task.Run(() => IsValidMethod(method)).ConfigureAwait(false))
						methods.Add(method);
				}
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