using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcTempEntitiesMessage : NetMessage
	{
		public int Entries { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Entries = buf.ReadBits(8);
			Length = buf.ReadBits(17);
			buf.SeekBits(Length);
			//Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(Entries, 8);
			bw.WriteBits(Length, 17);
			//bw.WriteBytes(Data);
			return Task.CompletedTask;
		}
	}
}