using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
    public class ConsoleCmd : DemoMessageType<ConsoleCmdMessage>
    {
        public ConsoleCmd(int code) : base(code)
        {
        }
    }
}
