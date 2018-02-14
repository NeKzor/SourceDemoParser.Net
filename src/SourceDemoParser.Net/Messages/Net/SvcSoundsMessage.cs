using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSoundsMessage : NetMessage
	{
		public bool ReliableSound { get; set; }
		public int Length { get; set; }
		public int Sounds { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			ReliableSound = buf.ReadBoolean();
			Sounds = (ReliableSound)
				? 1
				: buf.ReadBits(8);
			Length = (ReliableSound)
				? buf.ReadBits(8)
				: buf.ReadBits(16);
			buf.SeekBits(Length);
			//Data = buf.ReadBytes(Length);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBoolean(ReliableSound);
			if (ReliableSound) bw.WriteBits(Sounds, 8);
			bw.WriteBits(Length, (ReliableSound) ? 8 : 16);
			//bw.WriteBits(0, Length);
			return Task.CompletedTask;
		}
	}
}