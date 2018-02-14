using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPrint : NetMessageType<SvcPrintMessage>
	{
		public SvcPrint(int code) : base(code)
		{
		}
	}
}