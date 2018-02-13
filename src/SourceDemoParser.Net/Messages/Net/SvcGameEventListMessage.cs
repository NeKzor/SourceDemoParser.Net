using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGameEventListMessage : NetMessage
	{
		public int Events { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public SvcGameEventListMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Events = buf.ReadBits(9); // ?
			Length = buf.ReadBits(20); // ?
			buf.SeekBits(Length);
			//var data = buf.ReadBits(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(Events, 9);
			bw.WriteBits(Length, 20);
			//bw.WriteBits(0, Length);
			return Task.CompletedTask;
		}
	}
}