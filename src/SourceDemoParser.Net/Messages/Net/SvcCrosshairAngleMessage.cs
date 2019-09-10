using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
    public class SvcCrosshairAngleMessage : NetMessage
    {
        public QAngle Angle { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Angle = new QAngle(buf.ReadInt16(), buf.ReadInt16(), buf.ReadInt16());
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt16((short)Angle.X);
            bw.WriteInt16((short)Angle.Y);
            bw.WriteInt16((short)Angle.Z);
            return Task.CompletedTask;
        }
    }
}
