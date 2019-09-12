using SourceDemoParser.Engine;

namespace SourceDemoParser.Speedrun.Demos
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
        [StartAdjustment]
        public bool Mp_Start(PlayerCommand cmd)
        {
            // Not 100% sure if this works for coop
            var cmd1 = "dsp_player";
            var cmd2 = "ss_force_primary_fullscreen 0";
            return (cmd.Previous?.StartsWith(cmd1) == true) && (cmd.Current == cmd2);
        }

        [EndAdjustment("gg_stage_theend")]
        public bool GgStageTheend_Ending(PlayerCommand cmd)
        {
            var command = "playvideo_exitcommand_nointerrupt at_credits end_movie credits_video";
            return (cmd.Current == command);
        }
        [EndAdjustment]
        public bool Mp_Ending(PlayerCommand cmd)
        {
            // Not sure...
            var atlas = "playvideo_end_level_transition coop_bluebot_load 2";
            var pbody = "playvideo_end_level_transition coop_orangebot_load 2";
            return ((cmd.Current.StartsWith(atlas)) || (cmd.Current.StartsWith(pbody)));
        }
    }
}
