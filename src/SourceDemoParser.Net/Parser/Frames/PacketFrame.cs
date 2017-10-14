using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class PacketFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public List<PacketInfo> Infos { get; set; }
		public int InSequence { get; set; }
		public int OutSequence { get; set; }
		public byte[] NetData { get; set; }

		public PacketFrame()
		{
			Infos = new List<PacketInfo>(InternalParser.MAX_SPLITSCREEN_CLIENTS);
		}
		public PacketFrame(byte[] data) : this()
		{
			RawData = data;
		}

		Task IFrame.ParseData()
		{
			var buf = new BitBuffer(RawData);
			for (int i = 0; i < InternalParser.MAX_SPLITSCREEN_CLIENTS; i++)
			{
				// 76 bytes
				Infos.Add(new PacketInfo
				{
					Flags = (DemoFlags)buf.ReadInt32(),
					ViewOrigin = new Vector(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
					ViewAngles = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
					LocalViewAngles = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
					ViewOrigin2 = new Vector(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
					ViewAngles2 = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
					LocalViewAngles2 = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle())
				});
			}
			// 4 bytes
			var InSequence = buf.ReadInt32();
			// 4 bytes
			var OutSequence = buf.ReadInt32();

			var length = buf.ReadInt32();
			NetData = buf.ReadBytes(length);

			// TODO: Parse NET messages (game specific)
			//buf = new BitBuffer(NetData);
			//while (buf.BitsLeft > 6)
			//{
			//	var type = buf.ReadBits(6);
			//	var len = buf.ReadInt16();
			//}

			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = new byte[0];
			foreach (var info in Infos)
			{
				((int)info.Flags).GetBytes().AppendTo(ref bytes);
				info.ViewOrigin.GetBytes().AppendTo(ref bytes);
				info.ViewAngles.GetBytes().AppendTo(ref bytes);
				info.LocalViewAngles.GetBytes().AppendTo(ref bytes);
				info.ViewOrigin2.GetBytes().AppendTo(ref bytes);
				info.ViewAngles2.GetBytes().AppendTo(ref bytes);
				info.LocalViewAngles2.GetBytes().AppendTo(ref bytes);
			}
			InSequence.GetBytes().AppendTo(ref bytes);
			OutSequence.GetBytes().AppendTo(ref bytes);
			// TODO: Converted parsed NET data to bytes
			NetData.GetBytes().AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> Infos.FirstOrDefault()?.ViewOrigin?.ToString() ?? "NULL";
	}
}