using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetDisconnectMessage : NetMessage
    {
        public string Text { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Text = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteString(Text);
            return Task.CompletedTask;
        }
    }
}
