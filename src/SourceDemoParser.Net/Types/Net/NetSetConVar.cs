using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSetConVar : NetMessageType
	{
		public NetSetConVar(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new NetSetConVarMessage(this);
	}
}