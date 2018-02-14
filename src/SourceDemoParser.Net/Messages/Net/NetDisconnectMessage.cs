using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetDisconnectMessage : NetMessage
	{
		public string Reason { get; set; }
		
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Reason = buf.ReadString();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(Reason);
			return Task.CompletedTask;
		}
	}
}