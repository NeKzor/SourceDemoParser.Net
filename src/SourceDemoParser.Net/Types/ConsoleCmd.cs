using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class ConsoleCmd : DemoMessageType
	{
		public ConsoleCmd(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new ConsoleCmdMessage(this);
	}
}