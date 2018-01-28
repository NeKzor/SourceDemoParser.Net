using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public static class DemoMessageParsers
	{
		public static Task<IFrame> ParsePacket(BinaryReader br, SourceDemo demo)
		{
			var info = br.ReadBytes((demo.MaxSplitscreenClients * 76) + 4 + 4);
			var length = br.ReadInt32();
			var net = br.ReadBytes(length);

			return Task.FromResult(new PacketFrame(info, net) as IFrame);
		}
		public static Task<IFrame> ParseSyncTick(BinaryReader br, SourceDemo demo)
			=> Task.FromResult(default(IFrame));
		public static Task<IFrame> ParseConsoleCmdAsync(BinaryReader br, SourceDemo demo)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new ConsoleCmdFrame(data) as IFrame);
		}
		public static Task<IFrame> ParseUserCmd(BinaryReader br, SourceDemo demo)
		{
			var cmd = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new UserCmdFrame(cmd, data) as IFrame);
		}
		public static Task<IFrame> ParseDataTables(BinaryReader br, SourceDemo demo)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new DataTablesFrame(data) as IFrame);
		}
		public static Task<IFrame> ParseStop(BinaryReader br, SourceDemo demo)
			=> Task.FromResult(default(IFrame));
		public static Task<IFrame> ParseCustomData(BinaryReader br, SourceDemo demo)
		{
			var idk = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new CustomDataFrame(idk, data) as IFrame);
		}
		public static Task<IFrame> ParseStringTables(BinaryReader br, SourceDemo demo)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new StringTablesFrame(data) as IFrame);
		}
	}
}