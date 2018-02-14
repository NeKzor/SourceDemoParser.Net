using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class Stop : DemoMessageType<StopMessage>
	{
		public Stop(int code) : base(code)
		{
		}
	}
}