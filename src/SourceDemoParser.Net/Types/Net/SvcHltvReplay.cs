using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcHltvReplay : NetMessageType<SvcHltvReplayMessage>
	{
		public SvcHltvReplay(int code) : base(code)
		{
		}
	}
}