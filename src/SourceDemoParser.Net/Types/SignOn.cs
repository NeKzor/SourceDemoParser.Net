using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class SignOn : DemoMessageType
	{
		public SignOn(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new SignOnMessage(this);
	}
}