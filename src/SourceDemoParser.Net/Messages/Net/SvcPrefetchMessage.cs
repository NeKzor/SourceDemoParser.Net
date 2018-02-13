using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPrefetchMessage : INetMessage
	{
		public int SoundIndex { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(13);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}