using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcTempEntities : NetMessageType
	{
		public SvcTempEntities(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcTempEntitiesMessage(this);
	}
}