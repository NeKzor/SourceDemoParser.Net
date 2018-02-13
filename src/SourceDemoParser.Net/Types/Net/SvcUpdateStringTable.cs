using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcUpdateStringTable : NetMessageType
	{
		public SvcUpdateStringTable(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
			=> new SvcUpdateStringTableMessage(this);
	}
}