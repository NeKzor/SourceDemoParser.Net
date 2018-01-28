using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public static class DemoMessageExporters
	{
		public static Task<byte[]> ExportPacket(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as PacketFrame).PacketData.ToBytes().AppendTo(ref data);
			(frame as PacketFrame).NetData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public static Task<byte[]> ExportSyncTick(BinaryWriter bw, IFrame frame)
			=> Task.FromResult(default(byte[]));
		public static Task<byte[]> ExportConsoleCmd(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as ConsoleCmdFrame).RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public static Task<byte[]> ExportUserCmd(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as UserCmdFrame).CmdNumber.ToBytes().AppendTo(ref data);
			(frame as UserCmdFrame).RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public static Task<byte[]> ExportDataTables(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as DataTablesFrame).RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public static Task<byte[]> ExportStop(BinaryWriter bw, IFrame frame)
			=> Task.FromResult(default(byte[]));
		public static Task<byte[]> ExportCustomData(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as CustomDataFrame).Unknown1.ToBytes().AppendTo(ref data);
			(frame as CustomDataFrame).RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public static Task<byte[]> ExportStringTables(BinaryWriter bw, IFrame frame)
		{
			var data = new byte[0];
			(frame as StringTablesFrame).RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}