using System.Collections.Generic;
using System.IO;
using SourceDemoParser_CLI.Results;

namespace SourceDemoParser_CLI.Handlers
{
	public abstract class BaseGameHandler
    {
		public abstract SourceDemoProtocolVersion DemoProtocol { get; protected set; }
		public string FilePath { get; set; }
		public string GameDirectory { get; set; }
		public string MapName { get; set; }
		public int NetworkProtocol { get; set; }
		public string PlayerName { get; set; }
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
			GameInfo = Game.Unknown;
		}

        public static BaseGameHandler GetGameHandler(string gamedir, string mapname, SourceDemoProtocolVersion version)
        {
            if (gamedir == "portal")
			{
				return new PortalHandler();
			}
			if (gamedir == "portal2")
			{
				if (Game.Portal2Cooperative.HasMap(mapname))
					return new Portal2CooperativeHandler();
				if (Game.Portal2CooperativeDlc.HasMap(mapname))
					return new Portal2CooperativeDlcHandler();
				if (Game.Portal2SinglePlayer.HasMap(mapname))
					return new Portal2SinglePlayerHandler(Game.Portal2SinglePlayer);
				return new Portal2SinglePlayerHandler(Game.Portal2Workshop);
			}
			if (gamedir == "aperturetag")
			{
				if (Game.ApertureTag.HasMap(mapname))
					return new ApertureTagHandler(Game.ApertureTag);
				return new ApertureTagHandler(Game.ApertureTagWorkshop);
			}
			if (gamedir == "portal_stories")
			{
				return new PortalStoriesMelHandler();
			}
			if (gamedir == "infra")
			{
				if (Game.Infra.HasMap(mapname, true))
					return new InfraHandler(Game.Infra);
				return new InfraHandler(Game.InfraWorkshop);
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
        protected abstract long ProcessCustomData(BinaryReader br);
        protected abstract PacketResult ProcessPacket(BinaryReader br);
        protected abstract long ProcessStringTables(BinaryReader br);
        protected abstract long ProcessUserCmd(BinaryReader br);
		protected int ProcessSignOn(BinaryReader br)
		{
			br.BaseStream.Seek(SignOnLength, SeekOrigin.Current);
			return SignOnLength;
		}
	}
}