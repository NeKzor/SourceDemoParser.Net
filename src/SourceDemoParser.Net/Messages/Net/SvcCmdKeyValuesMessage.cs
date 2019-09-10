using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcCmdKeyValuesMessage : NetMessage
    {
        public byte[] Data { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            buf.SeekBytes(buf.ReadInt32());
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt32(Data.Length);
            bw.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
