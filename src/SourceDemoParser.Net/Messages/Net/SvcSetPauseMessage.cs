using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSetPauseMessage : NetMessage
    {
        public bool Paused { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Paused = buf.ReadBoolean();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBoolean(Paused);
            return Task.CompletedTask;
        }
    }
}
