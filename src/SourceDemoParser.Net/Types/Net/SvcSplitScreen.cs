using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSplitScreen : NetMessageType
	{
		public SvcSplitScreen(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcSplitScreenMessage(this);
	}
}