using System.Collections.Generic;
using System.IO;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	public abstract class BaseGameHandler
	{
		public abstract SourceDemoProtocolVersion DemoProtocol { get; protected set; }
		public string GameDirectory { get; set; }
		public string MapName { get; set; }
		public int NetworkProtocol { get; set; }
		public string Server { get; set; }
		public string Client { get; set; }
		public float Time { get; set; }
		public int TotalTicks { get; set; }
		public int FrameCount { get; set; }
		public int SignOnLength { get; set; }
		public Game GameInfo { get; set; }
		protected int CurrentTick { get; set; }
		protected string MapStartAdjustType { get; }
		protected string MapEndAdjustType { get; }
		protected List<ConsoleCommandFrame> ConsoleCommands { get; }
		protected List<PacketFrame> Packets { get; }

		protected BaseGameHandler()
		{
			MapStartAdjustType = "Map Start";
			MapEndAdjustType = "Map End";
			CurrentTick = -1;
			ConsoleCommands = new List<ConsoleCommandFrame>();
			Packets = new List<PacketFrame>();
			GameInfo = SupportedGames.Unknown;
		}

		public static BaseGameHandler GetGameHandler(string gamedir, string mapname, SourceDemoProtocolVersion version)
		{
			if (gamedir == "portal")
			{
				return new PortalHandler();
			}
			if (gamedir == "portal2")
			{
				if (SupportedGames.Portal2Cooperative.HasMap(mapname))
					return new Portal2CooperativeHandler();
				if (SupportedGames.Portal2CooperativeDlc.HasMap(mapname))
					return new Portal2CooperativeDlcHandler();
				if (SupportedGames.Portal2SinglePlayer.HasMap(mapname))
					return new Portal2SinglePlayerHandler(SupportedGames.Portal2SinglePlayer);
				return new Portal2SinglePlayerHandler(SupportedGames.Portal2Workshop);
			}
			if (gamedir == "aperturetag")
			{
				if (SupportedGames.ApertureTag.HasMap(mapname))
					return new ApertureTagHandler(SupportedGames.ApertureTag);
				return new ApertureTagHandler(SupportedGames.ApertureTagWorkshop);
			}
			if (gamedir == "portal_stories")
			{
				return new PortalStoriesMelHandler();
			}
			if (gamedir == "infra")
			{
				if (SupportedGames.Infra.HasMap(mapname, true))
					return new InfraHandler(SupportedGames.Infra);
				return new InfraHandler(SupportedGames.InfraWorkshop);
			}
			if (version == SourceDemoProtocolVersion.HL2)
				return new HL2Handler();
			if (version == SourceDemoProtocolVersion.ORANGEBOX)
				return new OrangeBoxHandler();
			return default(BaseGameHandler);
		}

		public abstract SourceDemo GetResult();
		public abstract long HandleCommand(byte command, int tick, BinaryReader br);
		public abstract bool IsStop(byte command);
		protected abstract ConsoleCmdResult ProcessConsoleCmd(BinaryReader br);
		protected abstract PacketResult ProcessPacket(BinaryReader br);
		protected abstract long ProcessStringTables(BinaryReader br);
		protected abstract long ProcessUserCmd(BinaryReader br);
		protected abstract long ProcessCustomData(BinaryReader br);
		protected int ProcessSignOn(BinaryReader br)
		{
			br.BaseStream.Seek(SignOnLength, SeekOrigin.Current);
			return SignOnLength;
		}
	}
}