using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcClassInfo : NetMessageType<SvcClassInfoMessage>
	{
		public SvcClassInfo(int code) : base(code)
		{
		}
	}
}