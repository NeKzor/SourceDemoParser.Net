using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcGetCvarValue : NetMessageType<SvcGetCvarValueMessage>
	{
		public SvcGetCvarValue(int code) : base(code)
		{
		}
	}
}