using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSignonState : NetMessageType
	{
		public NetSignonState(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetSignonStateMessage(this);
	}
}