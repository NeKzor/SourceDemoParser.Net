using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcMenuMessage : NetMessage
    {
        public short MenuType { get; set; }
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            MenuType = buf.ReadInt16();
            Data = buf.ReadBytes(buf.ReadInt32());
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt16(MenuType);
            bw.WriteInt32(Data.Length);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
