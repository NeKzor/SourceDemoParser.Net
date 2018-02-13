using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSetView : NetMessageType
	{
		public SvcSetView(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcSetViewMessage(this);
	}
}