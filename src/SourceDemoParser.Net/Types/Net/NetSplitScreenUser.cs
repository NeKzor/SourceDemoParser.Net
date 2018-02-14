using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSplitScreenUser : NetMessageType<NetSplitScreenUserMessage>
	{
		public NetSplitScreenUser(int code) : base(code)
		{
		}
	}
}