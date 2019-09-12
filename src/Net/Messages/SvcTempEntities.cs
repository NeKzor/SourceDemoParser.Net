using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcTempEntities : NetMessage
    {
        public int Entries { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Entries = buf.ReadBits(8);
            buf.SeekBits(buf.ReadBits(17));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(Entries, 8);
            buf.WriteBits(Data.Length, 17);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
