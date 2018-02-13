using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGetCvarValue : NetMessageType
	{
		public SvcGetCvarValue(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcGetCvarValueMessage(this);
	}
}