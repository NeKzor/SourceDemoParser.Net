using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetSetConVar : NetMessageType<NetSetConVarMessage>
	{
		public NetSetConVar(int code) : base(code)
		{
		}
	}
}