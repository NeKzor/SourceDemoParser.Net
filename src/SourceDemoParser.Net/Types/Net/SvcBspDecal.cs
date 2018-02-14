using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcBspDecal : NetMessageType<SvcBspDecalMessage>
	{
		public SvcBspDecal(int code) : base(code)
		{
		}
	}
}