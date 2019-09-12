using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcGameEventList : NetMessage
    {
        public int Events { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Events = buf.ReadBits(9);
            buf.SeekBits(buf.ReadBits(20));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(Events, 9);
            buf.WriteBits(Data.Length, 20);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
