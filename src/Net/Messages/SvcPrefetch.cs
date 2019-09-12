using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPrefetch : NetMessage
    {
        public int SoundIndex { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            SoundIndex = buf.ReadBits(13);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(SoundIndex, 13);
            return Task.CompletedTask;
        }
    }
}
