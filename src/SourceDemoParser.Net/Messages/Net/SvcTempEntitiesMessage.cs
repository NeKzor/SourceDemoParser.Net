using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcTempEntitiesMessage : NetMessage
    {
        public int Entries { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Entries = buf.ReadBits(8);
            buf.SeekBits(buf.ReadBits(17));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(Entries, 8);
            bw.WriteBits(Data.Length, 17);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
