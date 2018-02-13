using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPrefetch : NetMessageType
	{
		public SvcPrefetch(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcPrefetchMessage(this);
	}
}