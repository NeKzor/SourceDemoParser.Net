using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcEntityMessageMessage : NetMessage
    {
        public int EntityIndex { get; set; }
        public int ClassId { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            EntityIndex = buf.ReadBits(11);
            ClassId = buf.ReadBits(9);
            buf.SeekBits(buf.ReadBits(11));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(EntityIndex, 11);
            bw.WriteBits(ClassId, 9);
            bw.WriteBits(Data.Length, 11);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
