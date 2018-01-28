using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	internal static class Adjustments
	{
        internal static readonly Dictionary<TypeInfo, object> InstanceCache;
		internal static List<AdjustmentCache> AdjustmentCache;

        static Adjustments()
        {
            InstanceCache = new Dictionary<TypeInfo, object>();
            AdjustmentCache = new List<AdjustmentCache>();
        }

        internal static Task<object> GetInstance(TypeInfo t)
		{
			if (InstanceCache.TryGetValue(t, out object instance))
				return Task.FromResult(instance);
			throw new Exception($"Could not load instance cache of {t}.");
		}
        internal static Task AddToCache(TypeInfo iDemo)
		{
			// Note: ctor has to be public
			var instance = Activator.CreateInstance(iDemo.AsType(), null);
			if (instance == null)
				throw new Exception($"Failed to create instance of type: {iDemo}.");

			var demo = instance as ISourceDemo;
			if (demo == null)
				throw new Exception("Instance does not implement ISourceDemo.");

			// Cache stuff
			InstanceCache.Add(iDemo, instance);
			AdjustmentCache.Add(new AdjustmentCache()
			{
				Demo = demo,
				Adjustments = ConvertToAdjustment(iDemo).ToList()
			});
			return Task.CompletedTask;
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
					parameter = new PlayerPosition
					{
						Previous = cpos,
						Current = (cpos = ((PacketFrame)message.Frame).Infos[0].ViewOrigin)
					};
				}
				else if (adjustment.Parameter == PlayerStructType.Command)
				{
					parameter = new PlayerCommand
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
					throw new Exception($"[{message.CurrentTick}] Exception occured when invoking method: " +
						$"{adjustment.Method.Name}.\n{e}");
				}

				if (!result)
					continue;

				return new AdjustmentResult()
				{
					Found = true,
					FoundTickAt = message.CurrentTick
				};
			}
			return new AdjustmentResult();
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
			if ((typeof(ISourceDemo).GetTypeInfo().IsAssignableFrom(info))
				&& (info.IsClass) && (info.IsPublic) && !(info.IsAbstract))
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