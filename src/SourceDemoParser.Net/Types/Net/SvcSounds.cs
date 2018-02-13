using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSounds : NetMessageType
	{
		public SvcSounds(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcSoundsMessage(this);
	}
}