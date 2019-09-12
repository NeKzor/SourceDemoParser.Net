using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetFile : NetMessage
    {
        public uint TransferId { get; set; }
        public string FileName { get; set; }
        public bool FileRequested { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            TransferId = buf.ReadUInt32();
            FileName = buf.ReadString();
            FileRequested = buf.ReadBoolean();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.Write(TransferId);
            buf.Write(FileName);
            buf.Write(FileRequested);
            return Task.CompletedTask;
        }
    }
}
