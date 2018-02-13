using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcClassInfo : NetMessageType
	{
		public SvcClassInfo(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcClassInfoMessage(this);
	}
}