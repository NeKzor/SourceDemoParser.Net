using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcHltvReplayMessage : NetMessage
    {
        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
            => Task.CompletedTask;
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
            => Task.CompletedTask;
    }
}