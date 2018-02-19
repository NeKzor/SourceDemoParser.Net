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
		public static float GetIntervalPerTick(this SourceDemo demo)
			=> demo.PlaybackTime / demo.PlaybackTicks;

		// Data
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, DemoMessageType type)
			=> demo.Messages
				.Where(message => message.Type == type)
				.ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, string typeName)
			=> demo.Messages
				.Where(message => message.Type.Name == typeName)
				.ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByType(this SourceDemo demo, int typeCode)
			=> demo.Messages
				.Where(message => message.Type.MessageType == typeCode)
				.ToList();
		public static IReadOnlyCollection<IDemoMessage> GetMessagesByTick(this SourceDemo demo, int tick)
			=> demo.Messages
				.Where(message => message.Tick == tick)
				.ToList();
		public static Task ParseFrames(this SourceDemo demo)
			=> Task.Run(() => demo.Messages
				.ForEach(async (m) => await m.Frame.Parse(demo).ConfigureAwait(false)));

		// Adjustments
		public static Task<SourceDemo> AdjustExact(this SourceDemo demo, int endTick = 0, int startTick = 0)
		{
			if (demo.Messages.Count == 0)
				throw new InvalidOperationException("Cannot adjust demo without parsed messages.");
			
			var synced = false;
			var last = 0;
			foreach (var msg in demo.Messages)
			{
				if (msg.Type.Name == "SyncTick")
					synced = true;
				
				if (!synced)
					msg.Tick = 0;
				else if (msg.Tick < 0)
					msg.Tick = last;
				
				last = msg.Tick;
			}

			if (endTick < 1)
				endTick = demo.Messages.Last().Tick;

			var delta = endTick - startTick;
			if (delta < 0)
				throw new Exception("Start tick is greater than end tick.");

			var ipt = demo.GetIntervalPerTick();
			demo.PlaybackTicks = delta;
			demo.PlaybackTime = ipt * delta;
			return Task.FromResult(demo);
		}
		public static async Task<SourceDemo> AdjustFlagAsync(this SourceDemo demo, string saveFlag = "echo #SAVE#")
		{
			if (demo.Messages.Count == 0)
				throw new InvalidOperationException("Cannot adjust demo without parsed messages.");

			var flag = demo
				.GetMessagesByType("ConsoleCmd")
				.FirstOrDefault(m => (m.Frame as ConsoleCmdFrame).ConsoleCommand == saveFlag);
			
			if (flag != null)
			{
				await demo.AdjustExact(flag.Tick).ConfigureAwait(false);
				return demo;
			}
			return demo;
		}
	}
}