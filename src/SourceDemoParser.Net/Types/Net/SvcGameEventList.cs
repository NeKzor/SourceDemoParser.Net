using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGameEventList : NetMessageType
	{
		public SvcGameEventList(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcGameEventListMessage(this);
	}
}