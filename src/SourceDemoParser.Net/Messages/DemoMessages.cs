using System.Collections.Generic;
using DemoMessageTypes = System.Collections.Generic.List<SourceDemoParser.DemoMessageType>;

namespace SourceDemoParser
{
    public static class DemoMessages
	{
		public static DemoMessageTypes Default = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", DemoMessageParsers.ParsePacketAsync),
			new DemoMessageType("Packet", DemoMessageParsers.ParsePacketAsync),
			new DemoMessageType("SyncTick", DemoMessageParsers.ParseSyncTickAsync),
			new DemoMessageType("ConsoleCmd", DemoMessageParsers.ParseConsoleCmdAsync),
			new DemoMessageType("UserCmd", DemoMessageParsers.ParseUserCmdAsync),
			new DemoMessageType("DataTables", DemoMessageParsers.ParseDataTablesAsync),
			new DemoMessageType("Stop", DemoMessageParsers.ParseStopAsync),
			new DemoMessageType("CustomData", DemoMessageParsers.ParseCustomDataAsync),
			new DemoMessageType("StringTables", DemoMessageParsers.ParseStringTablesAsync)
		};
		public static DemoMessageTypes OldEngine = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", DemoMessageParsers.ParsePacketAsync),
			new DemoMessageType("Packet", DemoMessageParsers.ParsePacketAsync),
			new DemoMessageType("SyncTick", DemoMessageParsers.ParseSyncTickAsync),
			new DemoMessageType("ConsoleCmd", DemoMessageParsers.ParseConsoleCmdAsync),
			new DemoMessageType("UserCmd", DemoMessageParsers.ParseUserCmdAsync),
			new DemoMessageType("DataTables", DemoMessageParsers.ParseDataTablesAsync),
			new DemoMessageType("Stop", DemoMessageParsers.ParseStopAsync),
			new DemoMessageType("StringTables", DemoMessageParsers.ParseStringTablesAsync)
		};
	}
}