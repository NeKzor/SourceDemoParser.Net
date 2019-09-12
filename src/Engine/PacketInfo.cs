using System;

namespace SourceDemoParser.Engine
{
    public class PacketInfo
    {
        public DemoFlags Flags { get; set; }
        public Vector ViewOrigin { get; set; }
        public QAngle ViewAngles { get; set; }
        public QAngle LocalViewAngles { get; set; }
        public Vector ViewOrigin2 { get; set; }
        public QAngle ViewAngles2 { get; set; }
        public QAngle LocalViewAngles2 { get; set; }
    }

    [Flags]
    public enum DemoFlags
    {
        FDEMO_NORMAL = 0,
        FDEMO_USE_ORIGIN2 = 1,
        FDEMO_USE_ANGLES2 = 2,
        FDEMO_NOINTERP = 4
    }
}
