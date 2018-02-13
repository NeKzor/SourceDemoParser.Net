using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcServerInfo : NetMessageType
	{
		public SvcServerInfo(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcServerInfoMessage(this);
	}
}