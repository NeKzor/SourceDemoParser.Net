using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetNopMessage : INetMessage
	{
		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
			=> Task.CompletedTask;
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
			=> Task.FromResult(default(byte[]));
	}
}