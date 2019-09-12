using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcUpdateStringTable : NetMessage
    {
        public int TableId { get; set; }
        public short? NumChangedEntries { get; set; }
        public byte[] StringData { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            TableId = buf.ReadBits(5);
            if (buf.ReadBoolean())
                NumChangedEntries = buf.ReadInt16();
            buf.SeekBits(buf.ReadBits(20));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(TableId, 5);
            if (NumChangedEntries.HasValue)
            {
                buf.WriteBoolean(true);
                buf.WriteInt16(NumChangedEntries.Value);
            }
            else
            {
                buf.WriteBoolean(false);
            }
            buf.WriteBits(StringData.Length, 20);
            buf.WriteBytes(StringData);
            return Task.CompletedTask;
        }
    }
}
