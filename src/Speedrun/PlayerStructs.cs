using SourceDemoParser.Engine;

namespace SourceDemoParser.Speedrun
{
    public struct PlayerPosition
    {
        public Vector Previous;
        public Vector Current;
    }

    public struct PlayerCommand
    {
        public string Previous;
        public string Current;
    }
}
