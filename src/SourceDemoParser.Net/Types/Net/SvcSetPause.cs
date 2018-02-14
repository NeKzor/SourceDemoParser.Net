using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSetPause : NetMessageType<SvcSetPauseMessage>
	{
		public SvcSetPause(int code) : base(code)
		{
		}
	}
}