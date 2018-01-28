namespace SourceDemoParser.Extensions.Demos
{
	public class ApertureTagDemo : ISourceDemo
	{
		public string GameDirectory => "aperturetag";
		public uint DefaultTickrate => 60u;

		[StartAdjustment("gg_intro_wakeup")]
		public bool GgIntroWakeup_Start(PlayerPosition pos)
		{
			var destination = new Vector(-723.00f, -2481.00f, 17.00f);
			if ((pos.Previous == destination) && (pos.Current != destination))
				return true;
			return false;
		}

		[EndAdjustment("gg_stage_theend")]
		public bool GgStageTheend_Ending(PlayerCommand cmd)
		{
			var command = "playvideo_exitcommand_nointerrupt at_credits end_movie credits_video";
			return (cmd.Current == command);
		}
	}
}