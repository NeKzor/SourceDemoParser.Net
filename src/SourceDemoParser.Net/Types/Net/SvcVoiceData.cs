using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcVoiceData : NetMessageType<SvcVoiceDataMessage>
	{
		public SvcVoiceData(int code) : base(code)
		{
		}
	}
}