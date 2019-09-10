using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
    public class DataTables : DemoMessageType<DataTablesMessage>
    {
        public DataTables(int code) : base(code)
        {
        }
    }
}
