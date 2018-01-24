using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public abstract class SourceExporterBase : ISourceExporter
	{
        public ExportMode Mode { get; set; }

        protected SourceExporterBase(ExportMode mode)
        {
            Mode = mode;
        }

        public abstract Task ExportAsync(BinaryWriter bw, SourceDemo demo);
        public abstract Task<byte[]> HandleMessageAsync(IDemoMessage message);

		public virtual Task<byte[]> ExportConsoleCmd(ConsoleCmdFrame frame)
		{
			var data = new byte[0];
			frame.RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public virtual Task<byte[]> ExportCustomData(CustomDataFrame frame)
		{
			var data = new byte[0];
			frame.Unknown1.ToBytes().AppendTo(ref data);
			frame.RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public virtual Task<byte[]> ExportDataTables(DataTablesFrame frame)
		{
			var data = new byte[0];
			frame.RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public virtual Task<byte[]> ExportPacket(PacketFrame frame)
		{
			var data = new byte[0];
			frame.PacketData.ToBytes().AppendTo(ref data);
			frame.NetData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public virtual Task<byte[]> ExportStringTables(StringTablesFrame frame)
		{
			var data = new byte[0];
			frame.RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
		public virtual Task<byte[]> ExportUserCmd(UserCmdFrame frame)
		{
			var data = new byte[0];
			frame.CmdNumber.ToBytes().AppendTo(ref data);
			frame.RawData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}