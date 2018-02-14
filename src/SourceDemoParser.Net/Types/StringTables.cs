using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class StringTables : DemoMessageType<StringTablesMessage>
	{
		public StringTables(int code) : base(code)
		{
		}
	}
}