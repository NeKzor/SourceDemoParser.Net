using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class CustomData : DemoMessageType
	{
		public CustomData(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new CustomDataMessage(this);
	}
}