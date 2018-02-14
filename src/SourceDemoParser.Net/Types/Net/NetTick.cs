using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetTick : NetMessageType<NetTickMessage>
	{
		public NetTick(int code) : base(code)
		{
		}
	}
}