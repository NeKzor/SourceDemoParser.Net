using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetSplitScreenUserMessage : NetMessage
	{
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
			=> Task.CompletedTask;
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
			=> Task.FromResult(default(byte[]));
	}
}