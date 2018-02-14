using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcUserMessage : NetMessageType<SvcUserMessageMessage>
	{
		public SvcUserMessage(int code) : base(code)
		{
		}
	}
}