using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSendTableMessage : NetMessage
	{
		public bool NeedsDecoder { get; set; }
		public short Length { get; set; }
		public byte[] Data { get; set; }

		public SvcSendTableMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			NeedsDecoder = buf.ReadBoolean();
			Length = buf.ReadInt16();
			buf.SeekBits(Length);
			//Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBoolean(NeedsDecoder);
			bw.WriteInt16(Length);
			bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}