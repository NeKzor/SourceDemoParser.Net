using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSignonState : NetMessageType<NetSignonStateMessage>
	{
		public NetSignonState(int code) : base(code)
		{
		}
	}
}