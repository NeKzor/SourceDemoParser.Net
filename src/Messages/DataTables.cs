using System.Collections.Generic;
using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages
{
    public class DataTables : DemoMessage
    {
        public List<SendTable> Tables { get; set; } = new List<SendTable>();
        public List<ServerClassInfo> Classes { get; set; } = new List<ServerClassInfo>();

        public SourceBufferReader Buffer { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Buffer = new SourceBufferReader(buf.ReadBufferField());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBufferield(Buffer.Data);
            return Task.CompletedTask;
        }
    }
}
