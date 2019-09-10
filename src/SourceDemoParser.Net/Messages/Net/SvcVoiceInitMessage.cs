using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcVoiceInitMessage : NetMessage
    {
        public string VoiceCodec { get; set; }
        public byte Quality { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            VoiceCodec = buf.ReadString();
            Quality = buf.ReadByte();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteString(VoiceCodec);
            bw.WriteByte(Quality);
            return Task.CompletedTask;
        }
    }
}
