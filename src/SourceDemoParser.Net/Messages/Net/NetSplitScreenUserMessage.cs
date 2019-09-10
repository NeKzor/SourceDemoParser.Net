using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetSplitScreenUserMessage : NetMessage
    {
        public int Slot { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Slot = buf.ReadBits(1);
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt32(Slot);
            return Task.CompletedTask;
        }
    }
}
