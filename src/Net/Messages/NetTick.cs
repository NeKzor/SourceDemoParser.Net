using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetTick : NetMessage
    {
        public int Tick { get; set; }
        public short HostFrameTime { get; set; }
        public short HostFrameTimeStdDeviation { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Tick = buf.ReadInt32();
            HostFrameTime = buf.ReadInt16();
            HostFrameTimeStdDeviation = buf.ReadInt16();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt32(Tick);
            buf.WriteInt16(HostFrameTime);
            buf.WriteInt16(HostFrameTimeStdDeviation);
            return Task.CompletedTask;
        }
    }
}
