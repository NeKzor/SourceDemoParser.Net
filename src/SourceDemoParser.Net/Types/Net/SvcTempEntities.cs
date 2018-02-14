using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcTempEntities : NetMessageType<SvcTempEntitiesMessage>
	{
		public SvcTempEntities(int code) : base(code)
		{
		}
	}
}