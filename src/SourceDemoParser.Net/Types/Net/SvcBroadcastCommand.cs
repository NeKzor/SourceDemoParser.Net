using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcBroadcastCommand : NetMessageType<SvcBroadcastCommandMessage>
	{
		public SvcBroadcastCommand(int code) : base(code)
		{
		}
	}
}