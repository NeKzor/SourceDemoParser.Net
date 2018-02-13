using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetDisconnect : NetMessageType
	{
		public NetDisconnect(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetDisconnectMessage(this);
	}
}