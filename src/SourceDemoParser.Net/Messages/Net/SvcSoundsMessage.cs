using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSoundsMessage : NetMessage
    {
        public bool ReliableSound { get; set; }
        public uint Sounds { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
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
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBoolean(ReliableSound);
            if (ReliableSound) bw.WriteUBits(Sounds, 8);
            bw.WriteBits(Data.Length, (ReliableSound) ? 8 : 16);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
