using SourceDemoParser.Engine;

namespace SourceDemoParser.Speedrun.Demos
{
    public class PortalDemo : ISourceDemo
    {
        public string GameDirectory => "portal";
        public uint DefaultTickrate => 66u;

        [StartAdjustment("testchmb_a_00", 1)]
        public bool TestchmbA00_Start(PlayerPosition pos)
        {
            var destination = new Vector(-544f, -368.75f, 160f);
            return (pos.Current == destination);
        }

        [EndAdjustment("escape_02", 1)]
        public bool Escape02_Ending(PlayerCommand cmd)
        {
            var command = "startneurotoxins 99999";
            return (cmd.Current == command);
        }
    }
}
