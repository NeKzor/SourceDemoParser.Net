using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcEntityMessage : NetMessage
    {
        public int EntityIndex { get; set; }
        public int ClassId { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            EntityIndex = buf.ReadBits(11);
            ClassId = buf.ReadBits(9);
            buf.SeekBits(buf.ReadBits(11));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(EntityIndex, 11);
            buf.WriteBits(ClassId, 9);
            buf.WriteBits(Data.Length, 11);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
