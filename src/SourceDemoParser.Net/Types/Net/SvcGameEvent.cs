using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGameEvent : NetMessageType
	{
		public SvcGameEvent(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcGameEventMessage(this);
	}
}