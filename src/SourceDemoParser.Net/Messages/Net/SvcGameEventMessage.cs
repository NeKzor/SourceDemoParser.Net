using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGameEventMessage : INetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadBits(11); // ?
			var data = buf.ReadBytes(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}