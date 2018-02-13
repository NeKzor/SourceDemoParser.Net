using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetSignonStateMessage : INetMessage
	{
		public byte SignonState { get; set; }
		public int SpawnCount { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			SignonState = buf.ReadByte();
			SpawnCount = buf.ReadInt32();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteByte(SignonState);
			bw.WriteInt32(SpawnCount);
			return Task.CompletedTask;
		}
	}
}