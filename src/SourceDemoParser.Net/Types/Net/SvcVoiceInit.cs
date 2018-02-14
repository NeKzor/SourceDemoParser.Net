using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcVoiceInit : NetMessageType<SvcVoiceInitMessage>
	{
		public SvcVoiceInit(int code) : base(code)
		{
		}
	}
}