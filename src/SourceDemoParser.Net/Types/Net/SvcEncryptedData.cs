using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcEncryptedData : NetMessageType<SvcEncryptedDataMessage>
	{
		public SvcEncryptedData(int code) : base(code)
		{
		}
	}
}