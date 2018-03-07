using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetPlayerAvatarData : NetMessageType<NetPlayerAvatarDataMessage>
	{
		public NetPlayerAvatarData(int code) : base(code)
		{
		}
	}
}