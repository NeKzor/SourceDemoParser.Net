using System.Collections.Generic;
using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages
{
    public class Packet : DemoMessage
    {
        public List<PacketInfo> Info { get; set; } = new List<PacketInfo>();
        public int InSequence { get; set; }
        public int OutSequence { get; set; }
        public SourceBufferReader Buffer { get; set; }
        public List<INetMessage> NetMessages { get; set; } = new List<INetMessage>();

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            for (int i = 0; i < (demo.Game.MaxSplitscreenClients ?? 1); i++)
            {
                Info.Add(new PacketInfo
                {
                    Flags = (DemoFlags)buf.Read<int>(),
                    ViewOrigin = buf.Read<Vector>(),
                    ViewAngles = buf.Read<QAngle>(),
                    LocalViewAngles = buf.Read<QAngle>(),
                    ViewOrigin2 = buf.Read<Vector>(),
                    ViewAngles2 = buf.Read<QAngle>(),
                    LocalViewAngles2 = buf.Read<QAngle>()
                });
            }
            InSequence = buf.Read<int>();
            OutSequence = buf.Read<int>();
            Buffer = new SourceBufferReader(buf.ReadBufferField());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            foreach (var info in Info)
            {
                buf.Write((int)info.Flags);
                buf.Write(info.ViewOrigin);
                buf.Write(info.ViewAngles);
                buf.Write(info.LocalViewAngles);
                buf.Write(info.ViewOrigin2);
                buf.Write(info.ViewAngles2);
                buf.Write(info.LocalViewAngles2);
            }
            buf.Write(InSequence);
            buf.Write(OutSequence);
            buf.WriteBufferield(Buffer.Data);
            return Task.CompletedTask;
        }
    }
}
