using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcCreateStringTable : NetMessageType
	{
		public SvcCreateStringTable(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcCreateStringTableMessage(this);
	}
}