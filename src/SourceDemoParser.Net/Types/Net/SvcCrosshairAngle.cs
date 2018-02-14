using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCrosshairAngle : NetMessageType<SvcCrosshairAngleMessage>
	{
		public SvcCrosshairAngle(int code) : base(code)
		{
		}
	}
}