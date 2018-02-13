using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCmdKeyValues : NetMessageType
	{
		public SvcCmdKeyValues(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcCmdKeyValuesMessage(this);
	}
}