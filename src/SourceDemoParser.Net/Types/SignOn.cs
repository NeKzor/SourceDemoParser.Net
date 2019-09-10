using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
    public class SignOn : DemoMessageType<SignOnMessage>
    {
        public SignOn(int code) : base(code)
        {
        }
    }
}
