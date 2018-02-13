using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSoundsMessage : INetMessage
	{
		public bool ReliableSound { get; set; }
		public int Length { get; set; }
		public int Sounds { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var reliable = buf.ReadBoolean();
			var sounds = (reliable) ? 1 : buf.ReadBits(8);
			var length = (reliable) ? buf.ReadBits(8) : buf.ReadBits(16);
			buf.SeekBits(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}