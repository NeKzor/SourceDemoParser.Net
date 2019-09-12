using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSounds : NetMessage
    {
        public bool ReliableSound { get; set; }
        public uint Sounds { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            ReliableSound = buf.ReadBoolean();
            Sounds = (ReliableSound)
                ? 1
                : buf.ReadUBits(8);
            var length = (ReliableSound)
                ? buf.ReadUBits(8)
                : buf.ReadUBits(16);
            buf.SeekBits((int)length);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBoolean(ReliableSound);
            if (ReliableSound) buf.WriteUBits(Sounds, 8);
            buf.WriteBits(Data.Length, (ReliableSound) ? 8 : 16);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
