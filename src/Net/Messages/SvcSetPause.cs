using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcSetPause : NetMessage
    {
        public bool Paused { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Paused = buf.ReadBoolean();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBoolean(Paused);
            return Task.CompletedTask;
        }
    }
}
