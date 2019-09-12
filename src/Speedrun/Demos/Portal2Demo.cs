using System;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Speedrun.Demos
{
    public class Portal2Demo : ISourceDemo
    {
        public string GameDirectory => "portal2";
        public uint DefaultTickrate => 60u;

        [StartAdjustment("sp_a1_intro1", 1)]
        public bool SpA1Intro1_Start(PlayerPosition pos)
        {
            var destination = new Vector(-8709.20f, 1690.07f, 28.00f);
            var tolerance = new Vector(0.02f, 0.02f, 0.5f);
            return !(Math.Abs(pos.Current.X - destination.X) > tolerance.X)
                && !(Math.Abs(pos.Current.Y - destination.Y) > tolerance.Y)
                && !(Math.Abs(pos.Current.Z - destination.Z) > tolerance.Z);
        }
        [StartAdjustment("e1912", -2)]
        public bool E1912_Start(PlayerPosition pos)
        {
            var destination = new Vector(-655.748779296875f, -918.37353515625f, -4.96875f);
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
        [StartAdjustment("mp_coop_start")]
        public bool MpCoopStart_Start(PlayerPosition pos)
        {
            var atlas = new Vector(-9896f, -4400f, 3048f);
            var pbody = new Vector(-11168f, -4384f, 3040.03125f);
            if ((pos.Current == atlas) || (pos.Current == pbody))
                return true;
            return false;
        }

        [EndAdjustment("sp_a4_finale4", -852)]
        public bool SpA4Finale4_Ending(PlayerPosition pos)
        {
            var destination = new Vector(54.1f, 159.2f, -201.4f);
            var aa = Math.Pow(pos.Current.X - destination.X, 2);
            var bb = Math.Pow(pos.Current.Y - destination.Y, 2);
            var cc = Math.Pow(50, 2);
            return (((aa + bb) < cc) && (pos.Current.Z < destination.Z));
        }
        [EndAdjustment]
        public bool Mp_Ending(PlayerCommand cmd)
        {
            // Not sure...
            var atlas = "playvideo_end_level_transition coop_bluebot_load 2";
            var pbody = "playvideo_end_level_transition coop_orangebot_load 2";
            return ((cmd.Current.StartsWith(atlas)) || (cmd.Current.StartsWith(pbody)));
        }
        [EndAdjustment("mp_coop_paint_longjump_intro")]
        public bool MpCoopPaintLongjumpIntro_Ending(PlayerCommand cmd)
        {
            var command = "playvideo_exitcommand_nointerrupt coop_outro end_movie vault-movie_outro";
            return (cmd.Current == command);
        }
        [EndAdjustment("mp_coop_paint_crazy_box")]
        public bool MpCoopPaintCrazyBox_Ending(PlayerCommand cmd)
        {
            var command = "playvideo_exitcommand_nointerrupt dlc1_endmovie end_movie movie_outro";
            return (cmd.Current == command);
        }
    }
}
