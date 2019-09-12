using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages.Net
{
    public class SvcCrosshairAngle : NetMessage
    {
        public QAngle Angle { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Angle = new QAngle(buf.ReadInt16(), buf.ReadInt16(), buf.ReadInt16());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt16((short)Angle.X);
            buf.WriteInt16((short)Angle.Y);
            buf.WriteInt16((short)Angle.Z);
            return Task.CompletedTask;
        }
    }
}
