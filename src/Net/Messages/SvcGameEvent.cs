using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcGameEvent : NetMessage
    {
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            buf.SeekBits((int)buf.ReadUBits(11));
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(Data.Length, 11);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
