using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
    public class ConsoleCmd : DemoMessage
    {
        public string Command { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Command = buf.ReadStringField();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteStringField(Command.AsSpan());
            return Task.CompletedTask;
        }
    }
}
