using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcBspDecal : NetMessageType
	{
		public SvcBspDecal(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcBspDecalMessage(this);
	}
}