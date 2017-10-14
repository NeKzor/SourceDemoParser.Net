using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public static class InternalParser
	{
		public static List<string> SinglePlayerGames = new List<string>() { "hl2", "portal" };
		public static int MAX_SPLITSCREEN_CLIENTS = 2;  // const.h
		public static bool IsCsgoDemo;

		public static void ConfigureParser(this SourceDemo demo)
		{
			// SP vs MP
			MAX_SPLITSCREEN_CLIENTS = (SinglePlayerGames.Contains(demo.GameDirectory)) ? 1 : 2;
			IsCsgoDemo = string.Equals(demo.GameDirectory, "csgo", StringComparison.CurrentCultureIgnoreCase);
		}

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
		internal static Task<DataTablesFrame> ProcessDataTables(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new DataTablesFrame(data));
		}
		internal static Task<PacketFrame> ProcessPacket(BinaryReader br)
		{
			var data = br.ReadBytes((MAX_SPLITSCREEN_CLIENTS * 76) + 4 + 4);
			var length = br.ReadInt32();
			length.GetBytes().AppendTo(ref data);
			br.ReadBytes(length).AppendTo(ref data);
			return Task.FromResult(new PacketFrame(data));
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