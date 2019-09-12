using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetSplitScreenUser : NetMessage
    {
        public int Slot { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Slot = buf.ReadBits(1);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt32(Slot);
            return Task.CompletedTask;
        }
    }
}
