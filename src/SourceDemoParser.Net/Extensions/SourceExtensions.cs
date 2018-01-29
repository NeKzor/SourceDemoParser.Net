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

		public static IDemoMessage FindMessage(this SourceDemo demo, string command, string defaultName = "ConsoleCmd")
			=> demo.GetMessagesByType(defaultName)
				.FirstOrDefault(message => (message.Frame as ConsoleCmdFrame).ConsoleCommand == command);
		public static IDemoMessage FindMessage(this SourceDemo demo, Vector position, string defaultName = "Packet")
			=> demo.GetMessagesByType(defaultName)
				.FirstOrDefault(message => Vector.Equals((message.Frame as PacketFrame).Infos[0].ViewOrigin, position));

		public static Task ParseFrames(this SourceDemo demo)
			=> Task.Run(() => demo.Messages
				.ForEach(async (m) => await m.Frame.ParseData(demo).ConfigureAwait(false)));

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
	}
}