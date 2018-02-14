using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class SyncTick : DemoMessageType<SyncTickMessage>
	{
		public SyncTick(int code) : base(code)
		{
		}
	}
}