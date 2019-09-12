using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPaintmapData : NetMessage
    {
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            buf.SeekBits(buf.ReadInt32());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt32(Data.Length);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
