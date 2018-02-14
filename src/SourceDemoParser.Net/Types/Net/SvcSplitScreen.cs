using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSplitScreen : NetMessageType<SvcSplitScreenMessage>
	{
		public SvcSplitScreen(int code) : base(code)
		{
		}
	}
}