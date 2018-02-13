using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetTick : NetMessageType
	{
		public NetTick(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetTickMessage(this);
	}
}