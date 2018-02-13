using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class StringTables : DemoMessageType
	{
		public StringTables(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new StringTablesMessage(this);
	}
}