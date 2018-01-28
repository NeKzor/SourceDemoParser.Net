using System.Collections.Generic;
using DemoMessageTypes = System.Collections.Generic.List<SourceDemoParser.DemoMessageType>;
using Parser = SourceDemoParser.DemoMessageParsers;
using Exporter = SourceDemoParser.DemoMessageExporters;

namespace SourceDemoParser
{
    public static class DemoMessages
	{
		public static DemoMessageTypes Default = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", Parser.ParsePacket, Exporter.ExportPacket),
			new DemoMessageType("Packet", Parser.ParsePacket, Exporter.ExportPacket),
			new DemoMessageType("SyncTick", Parser.ParseSyncTick, Exporter.ExportSyncTick),
			new DemoMessageType("ConsoleCmd", Parser.ParseConsoleCmdAsync, Exporter.ExportConsoleCmd),
			new DemoMessageType("UserCmd", Parser.ParseUserCmd, Exporter.ExportUserCmd),
			new DemoMessageType("DataTables", Parser.ParseDataTables, Exporter.ExportDataTables),
			new DemoMessageType("Stop", Parser.ParseStop, Exporter.ExportStop),
			new DemoMessageType("CustomData", Parser.ParseCustomData, Exporter.ExportCustomData),
			new DemoMessageType("StringTables", Parser.ParseStringTables, Exporter.ExportStringTables)
		};
		public static DemoMessageTypes OldEngine = new DemoMessageTypes()
		{
			new DemoMessageType("SignOn", Parser.ParsePacket, Exporter.ExportPacket),
			new DemoMessageType("Packet", Parser.ParsePacket, Exporter.ExportPacket),
			new DemoMessageType("SyncTick", Parser.ParseSyncTick, Exporter.ExportSyncTick),
			new DemoMessageType("ConsoleCmd", Parser.ParseConsoleCmdAsync, Exporter.ExportConsoleCmd),
			new DemoMessageType("UserCmd", Parser.ParseUserCmd, Exporter.ExportUserCmd),
			new DemoMessageType("DataTables", Parser.ParseDataTables, Exporter.ExportDataTables),
			new DemoMessageType("Stop", Parser.ParseStop, Exporter.ExportStop),
			new DemoMessageType("StringTables", Parser.ParseStringTables, Exporter.ExportStringTables)
		};
	}
}