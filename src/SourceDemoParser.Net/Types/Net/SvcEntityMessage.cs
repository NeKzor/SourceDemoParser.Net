using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcEntityMessage : NetMessageType
	{
		public SvcEntityMessage(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcEntityMessageMessage(this);
	}
}