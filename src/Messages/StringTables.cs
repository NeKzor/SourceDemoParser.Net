using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages
{
    public class StringTables : DemoMessage
    {
        public SourceBufferReader Buffer { get; set; }
        public List<StringTable> Tables { get; set; } = new List<StringTable>();

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Buffer = new SourceBufferReader(buf.ReadBufferField());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBufferield(buf.Data);
            return Task.CompletedTask;
        }
    }
}
