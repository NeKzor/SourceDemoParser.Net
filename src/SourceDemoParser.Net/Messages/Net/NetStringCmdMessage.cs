using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetStringCmdMessage : NetMessage
    {
        public string Command { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Command = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteString(Command);
            return Task.CompletedTask;
        }
    }
}
