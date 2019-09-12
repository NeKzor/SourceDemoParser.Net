using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSplitScreen : NetMessage
    {
        public int Unk { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Unk = buf.ReadOneBit();
            buf.SeekBits(buf.ReadBits(11));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteOneBit(Unk);
            buf.WriteBits(Data.Length, 11);
            return Task.CompletedTask;
        }
    }
}
