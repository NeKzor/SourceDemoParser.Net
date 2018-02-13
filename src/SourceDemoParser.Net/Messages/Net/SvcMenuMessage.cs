using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcMenuMessage : NetMessage
	{
		public short MenuType { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }

		public SvcMenuMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			MenuType = buf.ReadInt16();
			Length = buf.ReadUInt32();
			Data = buf.ReadBytes((int)Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteInt16(MenuType);
			bw.WriteUInt32(Length);
			bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}