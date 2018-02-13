using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetSignonStateMessage : NetMessage
	{
		public byte SignonState { get; set; }
		public int SpawnCount { get; set; }
		
		public NetSignonStateMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			SignonState = buf.ReadByte();
			SpawnCount = buf.ReadInt32();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteByte(SignonState);
			bw.WriteInt32(SpawnCount);
			return Task.CompletedTask;
		}
	}
}