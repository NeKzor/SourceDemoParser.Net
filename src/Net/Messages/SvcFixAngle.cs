using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages.Net
{
    public class SvcFixAngle : NetMessage
    {
        public bool Relative { get; set; }
        public QAngle Angle { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Relative = buf.ReadBoolean();
            Angle = new QAngle
            (
                buf.ReadInt16(),
                buf.ReadInt16(),
                buf.ReadInt16()
            );
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBoolean(Relative);
            buf.WriteInt16((short)Angle.X);
            buf.WriteInt16((short)Angle.Y);
            buf.WriteInt16((short)Angle.Z);
            return Task.CompletedTask;
        }
    }
}
