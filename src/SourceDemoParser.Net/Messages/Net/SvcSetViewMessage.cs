using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSetViewMessage : INetMessage
	{
		public int EntityIndex { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(11);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}