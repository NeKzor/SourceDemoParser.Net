using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcFixAngle : NetMessageType
	{
		public SvcFixAngle(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcFixAngleMessage(this);
	}
}