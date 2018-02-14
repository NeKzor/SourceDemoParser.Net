using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGameEventMessage : NetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Length = buf.ReadBits(11); // ?
			//Data = buf.ReadByte(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(Length, 11);
			//bw.WriteBits(0, Length);
			return Task.CompletedTask;
		}
	}
}