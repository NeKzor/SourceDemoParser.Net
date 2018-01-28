using System.Collections.Generic;
using DemoMessageTypes = System.Collections.Generic.List<SourceDemoParser.DemoMessageType>;

namespace SourceDemoParser
{
    public static class DemoMessages
	{
		public static DemoMessageTypes Default = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", MessageParsers.ParsePacketAsync),
			new DemoMessageType("Packet", MessageParsers.ParsePacketAsync),
			new DemoMessageType("SyncTick", MessageParsers.ParseSyncTickAsync),
			new DemoMessageType("ConsoleCmd", MessageParsers.ParseConsoleCmdAsync),
			new DemoMessageType("UserCmd", MessageParsers.ParseUserCmdAsync),
			new DemoMessageType("DataTables", MessageParsers.ParseDataTablesAsync),
			new DemoMessageType("Stop", MessageParsers.ParseStopAsync),
			new DemoMessageType("CustomData", MessageParsers.ParseCustomDataAsync),
			new DemoMessageType("StringTables", MessageParsers.ParseStringTablesAsync)
		};
		public static DemoMessageTypes OldEngine = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", MessageParsers.ParsePacketAsync),
			new DemoMessageType("Packet", MessageParsers.ParsePacketAsync),
			new DemoMessageType("SyncTick", MessageParsers.ParseSyncTickAsync),
			new DemoMessageType("ConsoleCmd", MessageParsers.ParseConsoleCmdAsync),
			new DemoMessageType("UserCmd", MessageParsers.ParseUserCmdAsync),
			new DemoMessageType("DataTables", MessageParsers.ParseDataTablesAsync),
			new DemoMessageType("Stop", MessageParsers.ParseStopAsync),
			new DemoMessageType("StringTables", MessageParsers.ParseStringTablesAsync)
		};
	}
}