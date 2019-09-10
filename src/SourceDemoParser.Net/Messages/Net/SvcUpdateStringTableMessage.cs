using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcUpdateStringTableMessage : NetMessage
    {
        public int TableId { get; set; }
        public short? NumChangedEntries { get; set; }
        public byte[] StringData { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            TableId = buf.ReadBits(5);
            if (buf.ReadBoolean())
                NumChangedEntries = buf.ReadInt16();
            buf.SeekBits(buf.ReadBits(20));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(TableId, 5);
            if (NumChangedEntries.HasValue)
            {
                bw.WriteBoolean(true);
                bw.WriteInt16(NumChangedEntries.Value);
            }
            else
            {
                bw.WriteBoolean(false);
            }
            bw.WriteBits(StringData.Length, 20);
            bw.WriteBytes(StringData);
            return Task.CompletedTask;
        }
    }
}
