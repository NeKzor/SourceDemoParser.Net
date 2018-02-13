using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcUserMessage : NetMessageType
	{
		public SvcUserMessage(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcUserMessageMessage(this);
	}
}