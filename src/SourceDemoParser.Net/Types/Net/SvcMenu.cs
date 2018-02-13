using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcMenu : NetMessageType
	{
		public SvcMenu(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcMenuMessage(this);
	}
}