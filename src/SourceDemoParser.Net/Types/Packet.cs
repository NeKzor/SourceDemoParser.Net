using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
	public class Packet : DemoMessageType
	{
		public Packet(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
			=> new PacketMessage(this);
	}
}