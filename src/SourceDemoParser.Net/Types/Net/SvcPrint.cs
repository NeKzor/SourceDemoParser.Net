using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPrint : NetMessageType
	{
		public SvcPrint(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcPrintMessage(this);
	}
}