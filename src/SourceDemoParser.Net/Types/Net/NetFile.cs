using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetFile : NetMessageType<NetFileMessage>
	{
		public NetFile(int code) : base(code)
		{
		}
	}
}