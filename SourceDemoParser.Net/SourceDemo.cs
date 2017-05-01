using System;
using System.Collections.Generic;
using System.IO;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net
{
	public partial class SourceDemo : ICloneable
	{
		public SourceDemoProtocolVersion DemoProtocol { get; set; }
		public int NetworkProtocol { get; set; }
		public string FilePath { get; set; }
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
		public List<ConsoleCommandFrame> ConsoleCommands { get; set; }
		public List<PacketFrame> Packets { get; set; }
		// Extensions
		public string GetFileName()
			=> Path.GetFileName(FilePath);
		public string GetFileNameWithoutExtension()
			=> Path.GetFileNameWithoutExtension(FilePath);
		public float AdjustedTime
			=> AdjustedTicks * TicksPerSecond;
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
			ConsoleCommands = new List<ConsoleCommandFrame>();
			Packets = new List<PacketFrame>();
		}

		public object Clone()
		{
			return new SourceDemo()
			{
				DemoProtocol = DemoProtocol,
				NetworkProtocol = NetworkProtocol,
				FilePath = FilePath,
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