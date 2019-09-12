using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetStringCmd : NetMessage
    {
        public string Command { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Command = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteString(Command.AsSpan());
            return Task.CompletedTask;
        }
    }
}
