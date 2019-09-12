using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
    public class CustomData : DemoMessage
    {
        public int Unk { get; set; }
        public SourceBufferReader Buffer { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Unk = buf.Read<int>();
            Buffer = new SourceBufferReader(buf.ReadBufferField());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.Write(Unk);
            buf.WriteBufferield(Buffer.Data);
            return Task.CompletedTask;
        }
    }
}
