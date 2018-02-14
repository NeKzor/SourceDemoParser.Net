using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetDisconnect : NetMessageType<NetDisconnectMessage>
	{
		public NetDisconnect(int code) : base(code)
		{
		}
	}
}