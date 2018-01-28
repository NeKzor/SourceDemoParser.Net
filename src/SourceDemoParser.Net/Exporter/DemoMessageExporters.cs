using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public static class DemoMessageExporters
	{
		public static Task ExportPacket(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as PacketFrame).PacketData);
			bw.Write((frame as PacketFrame).PacketData);
			bw.Write((frame as PacketFrame).NetData.Length);
			return Task.CompletedTask;
		}
		public static Task ExportSyncTick(BinaryWriter bw, IFrame frame)
			=> Task.CompletedTask;
		public static Task ExportConsoleCmd(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as ConsoleCmdFrame).RawData.Length);
			bw.Write((frame as ConsoleCmdFrame).RawData);
			return Task.CompletedTask;
		}
		public static Task ExportUserCmd(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as UserCmdFrame).CmdNumber);
			bw.Write((frame as UserCmdFrame).RawData.Length);
			bw.Write((frame as UserCmdFrame).RawData);
			return Task.CompletedTask;
		}
		public static Task ExportDataTables(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as DataTablesFrame).RawData.Length);
			bw.Write((frame as DataTablesFrame).RawData);
			return Task.CompletedTask;
		}
		public static Task ExportStop(BinaryWriter bw, IFrame frame)
			=> Task.CompletedTask;
		public static Task ExportCustomData(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as CustomDataFrame).Unknown1);
			bw.Write((frame as CustomDataFrame).RawData.Length);
			bw.Write((frame as CustomDataFrame).RawData);
			return Task.CompletedTask;
		}
		public static Task ExportStringTables(BinaryWriter bw, IFrame frame)
		{
			bw.Write((frame as StringTablesFrame).RawData.Length);
			bw.Write((frame as StringTablesFrame).RawData);
			return Task.CompletedTask;
		}
	}
}