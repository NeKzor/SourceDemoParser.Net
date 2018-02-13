using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSetPauseMessage : NetMessage
	{
		public bool Paused { get; set; }

		public SvcSetPauseMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Paused = buf.ReadBoolean();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBoolean(Paused);
			return Task.CompletedTask;
		}
	}
}