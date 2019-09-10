using SourceDemoParser.Messages;

namespace SourceDemoParser.Types
{
    public class Packet : DemoMessageType<PacketMessage>
    {
        public Packet(int code) : base(code)
        {
        }
    }
}
