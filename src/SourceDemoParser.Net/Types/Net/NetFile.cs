using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetFile : NetMessageType
	{
		public NetFile(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetFileMessage(this);
	}
}