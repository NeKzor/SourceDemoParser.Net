using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSetPause : NetMessageType
	{
		public SvcSetPause(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcSetPauseMessage(this);
	}
}