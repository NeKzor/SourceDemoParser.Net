using SourceDemoParser_CLI.Helpers;

namespace SourceDemoParser_CLI.Results
{
	public class PacketFrame : IFrame
	{
		public Point3D PlayerPosition { get; set; }
		public int CurrentTick { get; set; }

		public PacketFrame()
		{
		}
		public PacketFrame(int tick, Point3D position)
		{
			CurrentTick = tick;
			PlayerPosition = position;
		}

		public override string ToString()
			=> $"{CurrentTick}\t{PlayerPosition}";
	}
}