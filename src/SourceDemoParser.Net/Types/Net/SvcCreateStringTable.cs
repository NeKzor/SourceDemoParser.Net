using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCreateStringTable : NetMessageType<SvcCreateStringTableMessage>
	{
		public SvcCreateStringTable(int code) : base(code)
		{
		}
	}
}