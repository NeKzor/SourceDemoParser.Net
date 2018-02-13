using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcUserMessageMessage : NetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public SvcUserMessageMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			//var type = buf.ReadBits(8);
			Length = buf.ReadBits(11);
			buf.SeekBits(Length);
			//Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(Length, 11);
			//bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}