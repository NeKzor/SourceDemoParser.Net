using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSetViewMessage : NetMessage
    {
        public int EntityIndex { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            EntityIndex = buf.ReadBits(11);
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(EntityIndex, 11);
            return Task.CompletedTask;
        }
    }
}
