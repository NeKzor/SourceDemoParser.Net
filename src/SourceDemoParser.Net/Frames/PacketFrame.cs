using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class PacketFrame : IFrame
	{
		public byte[] PacketData { get; set; }
		public byte[] NetData { get; set; }

		public List<PacketInfo> Infos { get; set; }
		public int InSequence { get; set; }
		public int OutSequence { get; set; }

		public PacketFrame()
		{
			Infos = new List<PacketInfo>();
		}
		public PacketFrame(byte[] packetData, byte[] netData) : this()
		{
			PacketData = packetData;
			NetData = netData;
		}

		Task IFrame.ParseData(SourceDemo demo)
		{
			var buf = new BitBuffer(PacketData);
			for (int i = 0; i < ((PacketData.Length - 8) / 76); i++)
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

			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			foreach (var info in Infos)
			{
				((int)info.Flags).ToBytes().AppendTo(ref data);
				info.ViewOrigin.ToBytes().AppendTo(ref data);
				info.ViewAngles.ToBytes().AppendTo(ref data);
				info.LocalViewAngles.ToBytes().AppendTo(ref data);
				info.ViewOrigin2.ToBytes().AppendTo(ref data);
				info.ViewAngles2.ToBytes().AppendTo(ref data);
				info.LocalViewAngles2.ToBytes().AppendTo(ref data);
			}
			InSequence.ToBytes().AppendTo(ref data);
			OutSequence.ToBytes().AppendTo(ref data);
			NetData.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}