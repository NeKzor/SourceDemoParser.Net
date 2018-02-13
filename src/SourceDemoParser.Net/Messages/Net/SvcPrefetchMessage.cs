using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPrefetchMessage : NetMessage
	{
		public int SoundIndex { get; set; }

		public SvcPrefetchMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			SoundIndex = buf.ReadBits(13);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(SoundIndex, 13);
			return Task.CompletedTask;
		}
	}
}