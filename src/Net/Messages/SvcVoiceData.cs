using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcVoiceData : NetMessage
    {
        public byte FromClient { get; set; }
        public byte Proximity { get; set; }
        public ushort Length { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            FromClient = buf.ReadByte();
            Proximity = buf.ReadByte();
            Length = buf.ReadUInt16();
            buf.SeekBits(Length);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteByte(FromClient);
            buf.WriteByte(Proximity);
            buf.WriteUInt16(Length);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
