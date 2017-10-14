using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	// Export parsed RawData to speed things up
	internal static class InternalExporter
	{
		internal static Task<byte[]> ExportConsoleCmd(ConsoleCmdFrame frame)
		{
			var bytes = frame.RawData.Length.GetBytes();
			frame.RawData.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}
		internal static Task<byte[]> ExportCustomData(CustomDataFrame frame)
		{
			var bytes = frame.Unknown1.GetBytes();
			frame.RawData.Length.GetBytes().AppendTo(ref bytes);
			frame.RawData.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}
		internal static Task<byte[]> ExportDataTables(DataTablesFrame frame)
		{
			var bytes = frame.RawData.Length.GetBytes();
			frame.RawData.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}
		internal static Task<byte[]> ExportPacket(PacketFrame frame)
		{
			return Task.FromResult(frame.RawData);
		}
		internal static Task<byte[]> ExportStringTables(StringTablesFrame frame)
		{
			var bytes = frame.RawData.Length.GetBytes();
			frame.RawData.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}
		internal static Task<byte[]> ExportUserCmd(UserCmdFrame frame)
		{
			var bytes = frame.CmdNumber.GetBytes();
			frame.RawData.Length.GetBytes().AppendTo(ref bytes);
			frame.RawData.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}
	}
}