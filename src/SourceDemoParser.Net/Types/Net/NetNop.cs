using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetNop : NetMessageType
	{
		public NetNop(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetNopMessage(this);
	}
}