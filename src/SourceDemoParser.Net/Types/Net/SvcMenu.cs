using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcMenu : NetMessageType<SvcMenuMessage>
	{
		public SvcMenu(int code) : base(code)
		{
		}
	}
}