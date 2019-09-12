using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
    public class SyncTick : DemoMessage
    {
        public override Task Read(SourceBufferReader buf, SourceDemo demo)
            => Task.CompletedTask;
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
            => Task.CompletedTask;
    }
}
