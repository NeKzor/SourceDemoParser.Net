using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcGameEventListMessage : NetMessage
    {
        public int Events { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Events = buf.ReadBits(9);
            buf.SeekBits(buf.ReadBits(20));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(Events, 9);
            bw.WriteBits(Data.Length, 20);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
