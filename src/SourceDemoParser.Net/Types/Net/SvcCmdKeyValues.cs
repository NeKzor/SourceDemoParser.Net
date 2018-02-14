using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCmdKeyValues : NetMessageType<SvcCmdKeyValuesMessage>
	{
		public SvcCmdKeyValues(int code) : base(code)
		{
		}
	}
}