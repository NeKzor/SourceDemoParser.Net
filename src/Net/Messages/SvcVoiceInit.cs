using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcVoiceInit : NetMessage
    {
        public string VoiceCodec { get; set; }
        public byte Quality { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            VoiceCodec = buf.ReadString();
            Quality = buf.ReadByte();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.Write(VoiceCodec);
            buf.Write(Quality);
            return Task.CompletedTask;
        }
    }
}
