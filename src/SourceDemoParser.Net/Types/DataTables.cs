using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class DataTables : DemoMessageType
	{
		public DataTables(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new DataTablesMessage(this);
	}
}