using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcEntityMessage : NetMessageType<SvcEntityMessageMessage>
	{
		public SvcEntityMessage(int code) : base(code)
		{
		}
	}
}