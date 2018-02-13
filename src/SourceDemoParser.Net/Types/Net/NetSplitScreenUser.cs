using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSplitScreenUser : NetMessageType
	{
		public NetSplitScreenUser(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetSplitScreenUserMessage(this);
	}
}