using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSendTable : NetMessageType
	{
		public SvcSendTable(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcSendTableMessage(this);
	}
}