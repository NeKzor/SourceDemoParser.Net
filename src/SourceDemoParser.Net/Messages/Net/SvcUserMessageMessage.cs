using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcUserMessageMessage : NetMessage
    {
        public byte MessageType { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            MessageType = buf.ReadByte();
            buf.SeekBits(buf.ReadBits(demo.Protocol == 4 ? 12 : 11));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(Data.Length, 11);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
