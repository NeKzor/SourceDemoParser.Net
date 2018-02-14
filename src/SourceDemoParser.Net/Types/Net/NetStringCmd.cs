using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetStringCmd : NetMessageType<NetStringCmdMessage>
	{
		public NetStringCmd(int code) : base(code)
		{
		}
	}
}