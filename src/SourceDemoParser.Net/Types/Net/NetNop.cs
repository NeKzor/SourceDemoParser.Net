using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetNop : NetMessageType<NetNopMessage>
	{
		public NetNop(int code) : base(code)
		{
		}
	}
}