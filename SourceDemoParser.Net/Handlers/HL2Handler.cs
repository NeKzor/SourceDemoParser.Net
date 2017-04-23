#pragma warning disable RCS1079
using System;
using System.IO;
using System.Text;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class HL2Handler : BaseGameHandler
	{
		public override SourceDemoProtocolVersion DemoProtocol { get; protected set; }

		public HL2Handler()
			=> DemoProtocol = SourceDemoProtocolVersion.HL2;

		public override SourceDemo GetResult()
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
				PlaybackTime = Time,
				PlaybackTicks = TotalTicks,
				FrameCount = FrameCount,
				SignOnLength = SignOnLength,
				StartAdjustmentType = MapStartAdjustType,
				EndAdjustmentType = MapEndAdjustType,
				ConsoleCommands = ConsoleCommands,
				Packets = Packets,
				GameInfo = GameInfo
			};
		}

		public override long HandleCommand(byte command, int tick, BinaryReader br)
		{
			if (CurrentTick == -1)
			{
				if (tick == 0)
					CurrentTick = tick;
			}
			else if ((tick > 0) && (tick > CurrentTick))
			{
				CurrentTick = tick;
			}

			Enum.IsDefined(typeof(HL2DemoCommands), (HL2DemoCommands)command);
			if (command == 1)
				return ProcessSignOn(br);
			if (command == 2)
				return ProcessPacket(br).Read;
			if (command == 3)
				return 0;
			if (command == 4)
				return ProcessConsoleCmd(br).Read;
			if (command == 5)
				return ProcessUserCmd(br);
			if (command == 6)
				throw new NotImplementedException();
			if (command != 8)
				throw new Exception($"Unknown command: 0x{command.ToString("X")}");
			return ProcessStringTables(br);
		}

		public override bool IsStop(byte command)
			=> command == 7;

		protected override ConsoleCmdResult ProcessConsoleCmd(BinaryReader br)
		{
			var position = br.BaseStream.Position;
			var command = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())).TrimEnd(new char[1]);
			ConsoleCommands.Add(new ConsoleCommandFrame(CurrentTick, command));
#if DEBUG
			System.Diagnostics.Trace.WriteLine($"[{CurrentTick}] : {command}");
#endif
			return new ConsoleCmdResult()
			{
				Read = br.BaseStream.Position - position,
				Command = command
			};
		}

		protected override long ProcessCustomData(BinaryReader br)
			=> throw new NotImplementedException();

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var position = br.BaseStream.Position;
			br.BaseStream.Seek(4, SeekOrigin.Current);
			var playerpos = new Point3D(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
			br.BaseStream.Seek(68, SeekOrigin.Current);
			var num = br.ReadInt32();
			br.BaseStream.Seek(num, SeekOrigin.Current);
			Packets.Add(new PacketFrame(CurrentTick, playerpos));
#if DEBUG
			System.Diagnostics.Trace.WriteLine($"[{CurrentTick}] : {playerpos}");
#endif
			return new PacketResult()
			{
				Read = br.BaseStream.Position - position,
				CurrentPosition = playerpos
			};
		}

		protected override long ProcessStringTables(BinaryReader br)
		{
			var position = br.BaseStream.Position;
			var num = br.ReadInt32();
			br.BaseStream.Seek(num, SeekOrigin.Current);
			return br.BaseStream.Position - position;
		}

		protected override long ProcessUserCmd(BinaryReader br)
		{
			var position = br.BaseStream.Position;
			br.BaseStream.Seek(4, SeekOrigin.Current);
			var num = br.ReadInt32();
			br.BaseStream.Seek(num, SeekOrigin.Current);
			return br.BaseStream.Position - position;
		}

		protected enum HL2DemoCommands
		{
			SignOn = 1,
			Packet,
			SyncTick,
			ConsoleCmd,
			UserCmd,
			DataTables,
			Stop,
			StringTables
		}
	}
}