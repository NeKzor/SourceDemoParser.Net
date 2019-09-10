using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPrefetchMessage : NetMessage
    {
        public int SoundIndex { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            SoundIndex = buf.ReadBits(13);
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(SoundIndex, 13);
            return Task.CompletedTask;
        }
    }
}
