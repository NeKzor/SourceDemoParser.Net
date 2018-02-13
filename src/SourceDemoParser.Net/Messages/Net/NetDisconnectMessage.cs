using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetDisconnectMessage : INetMessage
	{
		public string Reason { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Reason = buf.ReadString();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(Reason);
			return Task.CompletedTask;
		}
	}
}