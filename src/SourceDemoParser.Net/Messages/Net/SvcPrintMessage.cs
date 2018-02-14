using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPrintMessage : NetMessage
	{
		public string Text { get; set; }
		
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Text = buf.ReadString();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(Text);
			return Task.CompletedTask;
		}
	}
}