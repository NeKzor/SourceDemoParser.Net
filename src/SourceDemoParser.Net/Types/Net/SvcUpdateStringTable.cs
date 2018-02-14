using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcUpdateStringTable : NetMessageType<SvcUpdateStringTableMessage>
	{
		public SvcUpdateStringTable(int code) : base(code)
		{
		}
	}
}