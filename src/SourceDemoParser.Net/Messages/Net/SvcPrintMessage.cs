using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPrintMessage : NetMessage
    {
        public string Message { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Message = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteString(Message);
            return Task.CompletedTask;
        }
    }
}
