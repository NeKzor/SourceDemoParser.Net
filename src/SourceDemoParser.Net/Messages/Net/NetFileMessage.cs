using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetFileMessage : NetMessage
    {
        public uint TransferId { get; set; }
        public string FileName { get; set; }
        public bool FileRequested { get; set; }
        //public bool IsReplayDemoFile { get; set; }
        //public bool Deny { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            TransferId = buf.ReadUInt32();
            FileName = buf.ReadString();
            FileRequested = buf.ReadBoolean();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteUInt32(TransferId);
            bw.WriteString(FileName);
            bw.WriteBoolean(FileRequested);
            return Task.CompletedTask;
        }
    }
}
