using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPrintMessage : INetMessage
	{
		public string Text { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Text = buf.ReadString();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(Text);
			return Task.CompletedTask;
		}
	}
}