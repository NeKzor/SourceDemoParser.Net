using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetDisconnect : NetMessage
    {
        public string Text { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Text = buf.Read<string>();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.Write(Text);
            return Task.CompletedTask;
        }
    }
}
