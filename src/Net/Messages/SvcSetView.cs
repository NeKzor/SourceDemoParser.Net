using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSetView : NetMessage
    {
        public int EntityIndex { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            EntityIndex = buf.ReadBits(11);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(EntityIndex, 11);
            return Task.CompletedTask;
        }
    }
}
