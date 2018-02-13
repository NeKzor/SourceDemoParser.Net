using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcServerInfoMessage : NetMessage
	{
		public short Protocol { get; set; }
		public int ServerCount { get; set; }
		public bool IsHltv { get; set; }
		public bool IsDedicated { get; set; }
		public int ClientCrc { get; set; }
		public ushort MaxClasses { get; set; }
		public int MapCrc { get; set; }
		public byte PlayerSlot { get; set; }
		public byte MaxClients { get; set; }
		public float TickInterval { get; set; }
		public char OperatingSystem { get; set; }
		public string GameDir { get; set; }
		public string MapName { get; set; }
		public string SkyName { get; set; }
		public string HostName { get; set; }
		//public bool Replay { get; set; }

		public SvcServerInfoMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Protocol = buf.ReadInt16();
			ServerCount = buf.ReadInt32();
			IsHltv= buf.ReadBoolean();
			IsDedicated = buf.ReadBoolean();
			ClientCrc = buf.ReadInt32();
			MaxClasses = buf.ReadUInt16();
			MapCrc = (Protocol < 18)
				? buf.ReadInt32()
				: buf.ReadBits(128);
			PlayerSlot = buf.ReadByte();
			MaxClients = buf.ReadByte();
			TickInterval = buf.ReadSingle();
			OperatingSystem = buf.ReadChar();
			GameDir = buf.ReadString();
			MapName = buf.ReadString();
			MapName = buf.ReadString();
			HostName = buf.ReadString();
			//Replay = buf.ReadBoolean();

			System.Diagnostics.Debug.WriteLine("Protocol: " + Protocol);
			System.Diagnostics.Debug.WriteLine("HostName: " + HostName);
			//System.Diagnostics.Debug.WriteLine("Replay: " + Replay);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteInt16(Protocol);
			bw.WriteInt32(ServerCount);
			bw.WriteBoolean(IsHltv);
			bw.WriteBoolean(IsDedicated);
			bw.WriteInt32(ClientCrc);
			bw.WriteUInt16(MaxClasses);
			if (Protocol < 18)
				bw.WriteInt32(MapCrc);
			else
				bw.WriteBits(MapCrc, 128);
			bw.WriteByte(PlayerSlot);
			bw.WriteByte(MaxClients);
			//bw.WriteSingle(TickInterval);
			//bw.WriteChar(OperatingSystem);
			bw.WriteString(GameDir);
			bw.WriteString(MapName);
			bw.WriteString(MapName);
			bw.WriteString(HostName);
			//bw.WriteBoolean(Replay);
			return Task.CompletedTask;
		}
	}
}