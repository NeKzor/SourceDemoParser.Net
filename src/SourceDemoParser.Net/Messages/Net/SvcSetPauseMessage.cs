using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSetPauseMessage : INetMessage
	{
		public bool Paused { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var paused = buf.ReadBoolean();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}