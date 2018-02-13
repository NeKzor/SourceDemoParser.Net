using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcVoiceInit : NetMessageType
	{
		public SvcVoiceInit(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcVoiceInitMessage(this);
	}
}