using System.Collections.Generic;
using SourceDemoParser.Messages;
using SourceDemoParser.Types;
using DemoMessageTypes = System.Collections.Generic.List<SourceDemoParser.DemoMessageType>;

namespace SourceDemoParser
{
    public static class DemoMessages
    {
        public static DemoMessageTypes Default = new DemoMessageTypes()
        {
            new SignOn(0x01),
            new Packet(0x02),
            new SyncTick(0x03),
            new ConsoleCmd(0x04),
            new UserCmd(0x05),
            new DataTables(0x06),
            new Stop(0x07),
            new CustomData(0x08),
            new StringTables(0x09)
        };
        public static DemoMessageTypes OldEngine = new DemoMessageTypes()
        {
            new SignOn(0x01),
            new Packet(0x02),
            new SyncTick(0x03),
            new ConsoleCmd(0x04),
            new UserCmd(0x05),
            new DataTables(0x06),
            new Stop(0x07),
            new StringTables(0x08)
        };
    }
}
