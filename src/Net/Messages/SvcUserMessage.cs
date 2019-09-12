using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcUserMessage : NetMessage
    {
        public byte MessageType { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            MessageType = buf.ReadByte();
            buf.SeekBits(buf.ReadBits(demo.Protocol == 4 ? 12 : 11));
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
