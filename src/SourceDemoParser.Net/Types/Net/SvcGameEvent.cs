using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGameEvent : NetMessageType<SvcGameEventMessage>
	{
		public SvcGameEvent(int code) : base(code)
		{
		}
	}
}