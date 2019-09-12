using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPrint : NetMessage
    {
        public string Message { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Message = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteString(Message.AsSpan());
            return Task.CompletedTask;
        }
    }
}
