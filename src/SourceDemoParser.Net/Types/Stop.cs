using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class Stop : DemoMessageType
	{
		public Stop(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new StopMessage(this);
	}
}