using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPacketEntities : NetMessageType
	{
		public SvcPacketEntities(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcPacketEntitiesMessage(this);
	}
}