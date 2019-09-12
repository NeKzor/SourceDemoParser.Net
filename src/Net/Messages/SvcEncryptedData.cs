using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcEncryptedData : NetMessage
    {
        public override Task Read(SourceBufferReader buf, SourceDemo demo)
            => Task.CompletedTask;
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
            => Task.CompletedTask;
    }
}
