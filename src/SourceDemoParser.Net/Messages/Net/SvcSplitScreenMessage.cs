using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSplitScreenMessage : NetMessage
    {
        public int Unk { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Unk = buf.ReadOneBit();
            buf.SeekBits(buf.ReadBits(11));
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteOneBit(Unk);
            bw.WriteBits(Data.Length, 11);
            return Task.CompletedTask;
        }
    }
}
