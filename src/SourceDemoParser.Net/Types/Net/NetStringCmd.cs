using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetStringCmd : NetMessageType
	{
		public NetStringCmd(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetStringCmdMessage(this);
	}
}