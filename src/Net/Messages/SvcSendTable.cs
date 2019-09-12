using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSendTable : NetMessage
    {
        public bool NeedsDecoder { get; set; }
        public short Length { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            NeedsDecoder = buf.ReadBoolean();
            Length = buf.ReadInt16();
            buf.SeekBits(Length);
            //Data = buf.ReadBytes(Length);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBoolean(NeedsDecoder);
            buf.WriteInt16(Length);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
