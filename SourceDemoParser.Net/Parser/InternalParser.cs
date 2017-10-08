using System.IO;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	internal static class InternalParser
	{
		internal static Task<ConsoleCmdFrame> ProcessConsoleCmd(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new ConsoleCmdFrame(data));
		}
		internal static Task<CustomDataFrame> ProcessCustomData(BinaryReader br)
		{
			var idk = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new CustomDataFrame(idk, data));
		}
		//internal static Task<bool> ProcessDataTables(BinaryReader br)
		//{
		//	return Task.FromResult(false);
		//}
		internal static Task<PacketFrame> ProcessPacket(BinaryReader br)
		{
			var frame = new PacketFrame();
			// 84 bytes
			frame.Infos.Add(new PacketInfo
			{
				Flags = br.ReadInt32(),
				ViewOrigin = new Vector(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewAngles = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				LocalViewAngles = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewOrigin2 = new Vector(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewAngles2 = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				LocalViewAngles2 = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				InSequence = br.ReadInt32(),
				OutSequence = br.ReadInt32()
			});
			// 76 bytes (160 bytes)
			frame.Infos.Add(new PacketInfo
			{
				Flags = br.ReadInt32(),
				ViewOrigin = new Vector(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewAngles = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				LocalViewAngles = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewOrigin2 = new Vector(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				ViewAngles2 = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
				LocalViewAngles2 = new QAngle(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
			});
			var length = br.ReadInt32();
			frame.RawData = br.ReadBytes(length);
			return Task.FromResult(frame);
		}
		internal static Task<StringTablesFrame> ProcessStringTables(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new StringTablesFrame(data));
		}
		internal static Task<UserCmdFrame> ProcessUserCmd(BinaryReader br)
		{
			var cmd = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new UserCmdFrame(cmd, data));
		}
	}
}