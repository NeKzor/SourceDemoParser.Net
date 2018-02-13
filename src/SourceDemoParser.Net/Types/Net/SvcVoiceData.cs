using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcVoiceData : NetMessageType
	{
		public SvcVoiceData(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcVoiceDataMessage(this);
	}
}