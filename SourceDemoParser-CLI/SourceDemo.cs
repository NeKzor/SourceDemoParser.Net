using System;
using System.Collections.Generic;
using System.IO;
using SourceDemoParser_CLI.Results;

namespace SourceDemoParser_CLI
{
	public class SourceDemo : ICloneable
	{
		public SourceDemoProtocolVersion DemoProtocol { get; set; }
		public int NetworkProtocol { get; set; }
		public string FilePath { get; set; }
		public string GameDirectory { get; set; }
		public string MapName { get; set; }
		public string Client { get; set; }
		public float PlaybackTime { get; set; }
		public int PlaybackTicks { get; set; }
		public int FrameCount { get; set; }
		public int SignOnLength { get; set; }
		public Game GameInfo { get; set; }
		// Fixing the changelevel bug
		public int StartAdjustmentTick { get; set; }
		public string StartAdjustmentType { get; set; }
		public int EndAdjustmentTick { get; set; }
		public string EndAdjustmentType { get; set; }
		public int AdjustedTicks
		{
			get
			{
				if ((StartAdjustmentTick > -1) && (EndAdjustmentTick > -1))
					return EndAdjustmentTick - StartAdjustmentTick;
				if (StartAdjustmentTick > -1)
					return PlaybackTicks - StartAdjustmentTick;
				if (EndAdjustmentTick > -1)
					return EndAdjustmentTick;
				return PlaybackTicks;
			}
		}
		// Processed data
		public List<ConsoleCommandFrame> ConsoleCommands { get; set; } = new List<ConsoleCommandFrame>();
		public List<PacketFrame> Packets { get; set; } = new List<PacketFrame>();
		// Extensions
		public string GetFileName()
			=> Path.GetFileName(FilePath);
		public string GetFileNameWithoutExtension()
			=> Path.GetFileNameWithoutExtension(FilePath);
		public float AdjustTime(float ticksPerSecond)
			=> AdjustedTicks * ticksPerSecond;
		public int Tickrate
			=> (int)Math.Round(PlaybackTicks / PlaybackTime);
		public float TicksPerSecond
			=> PlaybackTime / PlaybackTicks;

		public SourceDemo()
		{
			StartAdjustmentTick = -1;
			EndAdjustmentTick = -1;
		}

		public object Clone()
		{
			return new SourceDemo()
			{
				DemoProtocol = DemoProtocol,
				NetworkProtocol = NetworkProtocol,
				FilePath = FilePath,
				MapName = MapName,
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