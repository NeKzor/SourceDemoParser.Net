using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SvcCrosshairAngleMessage : INetMessage
	{
		public QAngle Angle { get; set; }
	}
}