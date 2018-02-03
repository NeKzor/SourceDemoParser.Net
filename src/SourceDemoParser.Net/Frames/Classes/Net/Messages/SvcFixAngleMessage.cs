using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SvcFixAngleMessage : INetMessage
	{
		public bool Relative { get; set; }
		public QAngle Angle { get; set; }
	}
}