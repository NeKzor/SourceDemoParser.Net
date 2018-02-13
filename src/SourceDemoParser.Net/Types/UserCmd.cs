using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class UserCmd : DemoMessageType
	{
		public UserCmd(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new UserCmdMessage(this);
	}
}