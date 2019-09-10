using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcGameEventMessage : NetMessage
    {
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            buf.SeekBits((int)buf.ReadUBits(11));
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
