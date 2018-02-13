using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcServerInfoMessage : INetMessage
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

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var protocol = buf.ReadInt16();
			var count = buf.ReadInt32();
			var hltv = buf.ReadBoolean();
			var dedicated = buf.ReadBoolean();
			var client = buf.ReadInt32();
			var classes = buf.ReadUInt16();
			var mapcrc = (protocol < 18)
				? buf.ReadInt32()
				: buf.ReadBits(128);
			var slot = buf.ReadByte();
			var clients = buf.ReadByte();
			var tick = buf.ReadSingle();
			var os = buf.ReadChar();
			var dir = buf.ReadString();
			var map = buf.ReadString();
			var sky = buf.ReadString();
			var host = buf.ReadString();
			//var replay = buf.ReadBoolean();

			System.Diagnostics.Debug.WriteLine("protocol: " + protocol);
			System.Diagnostics.Debug.WriteLine("host: " + host);
			//System.Diagnostics.Debug.WriteLine("replay: " + replay);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}