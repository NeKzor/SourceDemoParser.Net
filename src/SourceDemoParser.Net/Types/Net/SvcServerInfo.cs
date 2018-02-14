using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcServerInfo : NetMessageType<SvcServerInfoMessage>
	{
		public SvcServerInfo(int code) : base(code)
		{
		}
	}
}