using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcEncryptedDataMessage : NetMessage
    {
        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
            => Task.CompletedTask;
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
            => Task.CompletedTask;
    }
}
