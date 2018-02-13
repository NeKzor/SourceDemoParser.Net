using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCmdKeyValuesMessage : NetMessage
	{
		public uint Length { get; set; }
		public byte[] Data { get; set; }

		public SvcCmdKeyValuesMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Length = buf.ReadUInt32();
			Data = buf.ReadBytes((int)Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteUInt32(Length);
			bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}