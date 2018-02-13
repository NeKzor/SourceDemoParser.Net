using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCrosshairAngle : NetMessageType
	{
		public SvcCrosshairAngle(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcCrosshairAngleMessage(this);
	}
}