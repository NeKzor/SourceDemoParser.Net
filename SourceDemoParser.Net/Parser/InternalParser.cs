using System;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser.Net.Extensions;

namespace SourceDemoParser.Net
{
	internal static class InternalParser
	{
		// Todo: parse everything
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
		internal static Task<PacketFrame> ProcessPacket(BinaryReader br, int netProc)
		{
			// Todo
			
			var idk = br.ReadBytes(4);
			//br.BaseStream.Seek(4, SeekOrigin.Current);
			var playerpos = new Vector3f(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
			var length = (netProc != 2001) && ((netProc == 7108) || (netProc == 1028)) ? 296 : 144;
			var data = br.ReadBytes(length);
			//br.BaseStream.Seek(length, SeekOrigin.Current);
			var length2 = br.ReadInt32();
			var data2 = br.ReadBytes(length2);
			//br.BaseStream.Seek(num, SeekOrigin.Current);
			return Task.FromResult(new PacketFrame(playerpos));
		}
		internal static Task<StringTablesFrame> ProcessStringTables(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new StringTablesFrame(data));
		}
		internal static Task<UserCmdFrame> ProcessUserCmd(BinaryReader br)
		{
			var idk = br.ReadBytes(4);
			//br.BaseStream.Seek(4, SeekOrigin.Current);
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(new UserCmdFrame(idk, data));
		}
		//internal static Task<int> ProcessSignOn(BinaryReader br, int signOnLength)
		//{
		//	br.BaseStream.Seek(signOnLength, SeekOrigin.Current);
		//	return Task.FromResult(signOnLength);
		//}
		//internal static Task<bool> ProcessDataTables(BinaryReader br)
		//{
		//	return Task.FromResult(0);
		//}
	}
}