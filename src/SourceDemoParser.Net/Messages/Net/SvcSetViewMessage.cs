using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSetViewMessage : NetMessage
	{
		public int EntityIndex { get; set; }

		public SvcSetViewMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			EntityIndex = buf.ReadBits(11);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(EntityIndex, 11);
			return Task.CompletedTask;
		}
	}
}