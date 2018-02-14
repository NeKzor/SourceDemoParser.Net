using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPrefetch : NetMessageType<SvcPrefetchMessage>
	{
		public SvcPrefetch(int code) : base(code)
		{
		}
	}
}