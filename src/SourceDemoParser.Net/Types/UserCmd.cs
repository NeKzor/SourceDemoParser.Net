using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
    public class UserCmd : DemoMessageType<UserCmdMessage>
    {
        public UserCmd(int code) : base(code)
        {
        }
    }
}
