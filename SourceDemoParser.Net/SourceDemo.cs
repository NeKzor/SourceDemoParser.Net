using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net
{
	public partial class SourceDemo : ICloneable
	{
		public SourceDemoProtocolVersion DemoProtocol { get; set; }
		public int NetworkProtocol { get; set; }
		public string GameDirectory { get; set; }
		public string MapName { get; set; }
		public string Server { get; set; }
		public string Client { get; set; }
		public float PlaybackTime { get; set; }
		public int PlaybackTicks { get; set; }
		public int FrameCount { get; set; }
		public int SignOnLength { get; set; }
		public Game GameInfo { get; set; }
		public int StartAdjustmentTick { get; set; }
		public string StartAdjustmentType { get; set; }
		public int EndAdjustmentTick { get; set; }
		public string EndAdjustmentType { get; set; }
		public int AdjustedTicks
			=> AdjustTicks(StartAdjustmentTick, EndAdjustmentTick);
		public int AdjustTicks(int start, int end)
		{
			if ((start > -1) && (end > -1))
				return end - start;
			if (start > -1)
				return (ConsoleCommands.Last()?.CurrentTick ?? 0) - start;
			if (end > -1)
				return end;
			return ConsoleCommands.Last()?.CurrentTick ?? 0;
		}
		// Processed data
		public List<ConsoleCommandFrame> ConsoleCommands { get; set; }
		public List<PacketFrame> Packets { get; set; }
		// Extensions
		public float GetAdjustedTime()
			=> AdjustedTicks * GetTicksPerSecond();
		public float GetAdjustTime(float ticksPerSecond)
			=> AdjustedTicks * ticksPerSecond;
		public int GetTickrate()
			=> (int)Math.Round(PlaybackTicks / PlaybackTime);
		public float GetTicksPerSecond()
			=> PlaybackTime / PlaybackTicks;

		public SourceDemo()
		{
			StartAdjustmentTick = -1;
			EndAdjustmentTick = -1;
			ConsoleCommands = new List<ConsoleCommandFrame>();
			Packets = new List<PacketFrame>();
		}

		public object Clone()
		{
			return new SourceDemo()
			{
				DemoProtocol = DemoProtocol,
				NetworkProtocol = NetworkProtocol,
				MapName = MapName,
				Server = Server,
				Client = Client,
				GameDirectory = GameDirectory,
				PlaybackTime = PlaybackTime,
				PlaybackTicks = PlaybackTicks,
				FrameCount = FrameCount,
				SignOnLength = SignOnLength,
				StartAdjustmentTick = StartAdjustmentTick,
				StartAdjustmentType = StartAdjustmentType,
				EndAdjustmentTick = EndAdjustmentTick,
				EndAdjustmentType = EndAdjustmentType,
				ConsoleCommands = ConsoleCommands,
				Packets = Packets,
				GameInfo = GameInfo
			};
		}
	}
}