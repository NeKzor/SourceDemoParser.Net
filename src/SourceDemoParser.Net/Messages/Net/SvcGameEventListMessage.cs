using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGameEventListMessage : INetMessage
	{
		public int Events { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var events = buf.ReadBits(9); // ?
			var length = buf.ReadBits(20); // ?
			buf.SeekBits(length);
			//var data = buf.ReadBytes(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}