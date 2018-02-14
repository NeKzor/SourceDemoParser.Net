using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcUpdateStringTableMessage : NetMessage
	{
		public int Id { get; set; }
		public bool EntriesChanged { get; set; }
		public short ChangedEntries { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Id = buf.ReadBits(5);
			EntriesChanged = buf.ReadBoolean();
			ChangedEntries = (EntriesChanged)
				? buf.ReadInt16()
				: (short)1;
			Length = buf.ReadBits(20);
			buf.SeekBits(Length);
			//Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(Id, 5);
			bw.WriteBoolean(EntriesChanged);
			if (EntriesChanged) bw.WriteInt16(ChangedEntries);
			bw.WriteBits(Length, 20);
			//bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}