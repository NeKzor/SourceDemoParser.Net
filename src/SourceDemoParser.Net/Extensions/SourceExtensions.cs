using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	public static class SourceExtensions
	{
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
		
		public static void ParseFrames(this SourceDemo demo)
			=> demo.Messages.ForEach(async (m) => await m.Frame.ParseData(demo).ConfigureAwait(false));

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
			if (Adjustments.AdjustmentCache.Count == 0)
				throw new Exception("No adjustments have been loaded.");

			var tickrate = demo.GetTickrate();
			var candidates = new List<AdjustmentCache>();
			foreach (var cache in Adjustments.AdjustmentCache)
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
				var starts = adjustments
					.Where(a => a.Type == AdjustmentType.Start && a.MapName == demo.MapName);
				var iscommon = default(bool);

				if (!starts.Any())
				{
					iscommon = true;
					starts = adjustments
						.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = await Adjustments.GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
				}
				else
					starttick = await Adjustments.GetTickAsync(starts, packets, cmds).ConfigureAwait(false);

				if ((starttick == null) && (!iscommon))
				{
					starts = adjustments
						.Where(a => a.Type == AdjustmentType.Start && string.IsNullOrEmpty(a.MapName));
					if (starts.Any())
						starttick = await Adjustments.GetTickAsync(starts, packets, cmds).ConfigureAwait(false);
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
						endtick = await Adjustments.GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
				}
				else
					endtick = await Adjustments.GetTickAsync(ends, packets, cmds).ConfigureAwait(false);

				if ((endtick == null) && (!iscommon))
				{
					ends = adjustments
						.Where(a => a.Type == AdjustmentType.End && string.IsNullOrEmpty(a.MapName));
					if (ends.Any())
						endtick = await Adjustments.GetTickAsync(ends, packets, cmds).ConfigureAwait(false);
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
		// Returns true if new adjustments have been loaded
		public static async Task<bool> DiscoverAsync(Assembly asm = null)
		{
			asm = asm ?? typeof(SourceExtensions).GetTypeInfo().Assembly;
			var loaded = 0u;
			foreach (var type in asm.DefinedTypes)
			{
				if (!Adjustments.IsValidClass(type))
					continue;
				if (Adjustments.InstanceCache.ContainsKey(type))
					continue;

				await Adjustments.AddToCache(type).ConfigureAwait(false);
				++loaded;
			}
			return loaded != 0u;
		}
		// Returns true on success
		public static async Task<bool> LoadAsync<T>()
		{
			var type = typeof(T).GetTypeInfo();
			if (Adjustments.InstanceCache.ContainsKey(type))
				return false;

			if (!Adjustments.IsValidClass(type))
				return false;

			await Adjustments.AddToCache(type).ConfigureAwait(false);
			return true;
		}
	}
}