using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
    public class Stop : DemoMessage
    {
        public byte[] Rest { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Rest = (buf.BytesLeft > 0) ? buf.ReadBytes(buf.BytesLeft) : default;
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            if (Rest != null) buf.Write(Rest);
            return Task.CompletedTask;
        }
    }
}
