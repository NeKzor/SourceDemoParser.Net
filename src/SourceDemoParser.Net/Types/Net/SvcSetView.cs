using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSetView : NetMessageType<SvcSetViewMessage>
	{
		public SvcSetView(int code) : base(code)
		{
		}
	}
}