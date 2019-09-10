using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetTickMessage : NetMessage
    {
        public int Tick { get; set; }
        public short HostFrameTime { get; set; }
        public short HostFrameTimeStdDeviation { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Tick = buf.ReadInt32();
            HostFrameTime = buf.ReadInt16();
            HostFrameTimeStdDeviation = buf.ReadInt16();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt32(Tick);
            bw.WriteInt16(HostFrameTime);
            bw.WriteInt16(HostFrameTimeStdDeviation);
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{Type.Name}\n"
                + $"{nameof(Tick)} -> {Tick}\n"
                + $"{nameof(HostFrameTime)} -> {HostFrameTime}\n"
                + $"{nameof(HostFrameTimeStdDeviation)} -> {HostFrameTimeStdDeviation}";
        }
    }
}
