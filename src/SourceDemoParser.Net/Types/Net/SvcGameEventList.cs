using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGameEventList : NetMessageType<SvcGameEventListMessage>
	{
		public SvcGameEventList(int code) : base(code)
		{
		}
	}
}