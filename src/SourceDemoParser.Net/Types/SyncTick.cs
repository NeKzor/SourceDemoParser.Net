using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class SyncTick : DemoMessageType
	{
		public SyncTick(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new SyncTickMessage(this);
	}
}