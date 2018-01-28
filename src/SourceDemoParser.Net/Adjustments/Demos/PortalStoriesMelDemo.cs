namespace SourceDemoParser.Extensions.Demos
{
	public class PortalStoriesMelDemo : ISourceDemo
	{
		public string GameDirectory => "portal_stories";
		public uint DefaultTickrate => 60u;

		[StartAdjustment("sp_a1_tramride")]
		public bool StartA(PlayerPosition pos) => TramrideStart(pos);
		[StartAdjustment("st_a1_tramride")]
		public bool StartB(PlayerPosition pos) => TramrideStart(pos);
		[EndAdjustment("sp_a4_finale")]
		public bool EndingA(PlayerCommand cmd) => FinaleEnding(cmd);
		[EndAdjustment("st_a4_finale")]
		public bool EndingB(PlayerCommand cmd) => FinaleEnding(cmd);

		public bool TramrideStart(PlayerPosition pos)
		{
			var destination = new Vector(-4592.00f, -4475.4052734375f, 108.683975219727f);
			if ((pos.Previous == destination) && (pos.Current != destination))
				return true;
			return false;
		}
		public bool FinaleEnding(PlayerCommand cmd)
		{
			var command = "playvideo_exitcommand_nointerrupt aegis_interior.bik end_movie movie_aegis_interior";
			return (cmd.Current == command);
		}
	}
}