using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcFixAngle : NetMessageType<SvcFixAngleMessage>
	{
		public SvcFixAngle(int code) : base(code)
		{
		}
	}
}