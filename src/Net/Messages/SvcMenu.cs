using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcMenu : NetMessage
    {
        public short MenuType { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            MenuType = buf.ReadInt16();
            Data = buf.ReadBytes(buf.ReadInt32());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt16(MenuType);
            buf.WriteInt32(Data.Length);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
