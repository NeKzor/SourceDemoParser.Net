using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class CustomData : DemoMessageType<CustomDataMessage>
	{
		public CustomData(int code) : base(code)
		{
		}
	}
}