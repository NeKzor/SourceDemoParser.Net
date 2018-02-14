using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcSounds : NetMessageType<SvcSoundsMessage>
	{
		public SvcSounds(int code) : base(code)
		{
		}
	}
}