using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSendTable : NetMessageType<SvcSendTableMessage>
	{
		public SvcSendTable(int code) : base(code)
		{
		}
	}
}