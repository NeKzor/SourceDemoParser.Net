using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcVoiceDataMessage : NetMessage
	{
		public byte FromClient { get; set; }
		public byte Proximity { get; set; }
		public ushort Length { get; set; }
		public byte[] Data { get; set; }

		public SvcVoiceDataMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			FromClient = buf.ReadByte();
			Proximity = buf.ReadByte();
			Length = buf.ReadUInt16();
			Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteByte(FromClient);
			bw.WriteByte(Proximity);
			bw.WriteUInt16(Length);
			bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}