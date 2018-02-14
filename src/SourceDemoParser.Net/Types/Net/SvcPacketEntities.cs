using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPacketEntities : NetMessageType<SvcPacketEntitiesMessage>
	{
		public SvcPacketEntities(int code) : base(code)
		{
		}
	}
}