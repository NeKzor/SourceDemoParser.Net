using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPacketEntitiesMessage : NetMessage
	{
		public int MaxEntries { get; set; }
		public bool IsDelta { get; set; }
		public int DeltaFrom { get; set; }
		public bool BaseLine { get; set; }
		public int UpdatedEntries { get; set; }
		public int Length { get; set; }
		public bool UpdateBaseline { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			MaxEntries = buf.ReadBits(11); // ?
			IsDelta = buf.ReadBoolean();
			DeltaFrom = (IsDelta) ? buf.ReadInt32() : 0;
			//var length = buf.ReadUInt32();
			BaseLine = buf.ReadBoolean();
			UpdatedEntries = buf.ReadBits(11); // ?
			Length = buf.ReadBits(20);
			UpdateBaseline = buf.ReadBoolean();
			buf.SeekBits(Length);
			//Data = buf.ReadByte(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(MaxEntries, 11);
			bw.WriteBoolean(IsDelta);
			if (IsDelta) bw.WriteInt32(DeltaFrom);
			bw.WriteBoolean(BaseLine);
			bw.WriteBits(UpdatedEntries, 11);
			bw.WriteBits(Length, 20);
			bw.WriteBoolean(UpdateBaseline);
			bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}