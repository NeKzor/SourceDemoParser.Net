using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public enum DemoFlags
	{
		FDEMO_NORMAL = 0,
		FDEMO_USE_ORIGIN2 = 1,
		FDEMO_USE_ANGLES2 = 2,
		FDEMO_NOINTERP = 4
	}

	public class PacketInfo
	{
		public int Flags { get; set; }
		public Vector ViewOrigin { get; set; }
		public QAngle ViewAngles { get; set; }
		public QAngle LocalViewAngles { get; set; }
		public Vector ViewOrigin2 { get; set; }
		public QAngle ViewAngles2 { get; set; }
		public QAngle LocalViewAngles2 { get; set; }
		public int? InSequence { get; set; }
		public int? OutSequence { get; set; }
	}

	public class PacketFrame : IFrame
	{
		public byte[] RawData { get; set; }				// NET Data
		public List<PacketInfo> Infos { get; set; }

		public PacketFrame()
		{
			Infos = new List<PacketInfo>(Const.MAX_SPLITSCREEN_CLIENTS);
		}

		Task IFrame.ParseData()
		{
			var buf = new BitBuffer(RawData);
			while (buf.BitsLeft > 6)
			{
				var type = buf.ReadBits(6);
				// TODO: Parse NET messages (game specific)
			}

			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = new byte[0];
			foreach (var player in Infos)
			{
				player.Flags.GetBytes().AppendTo(ref bytes);
				player.ViewOrigin.GetBytes().AppendTo(ref bytes);
				player.ViewAngles.GetBytes().AppendTo(ref bytes);
				player.LocalViewAngles.GetBytes().AppendTo(ref bytes);
				player.ViewOrigin2.GetBytes().AppendTo(ref bytes);
				player.ViewAngles2.GetBytes().AppendTo(ref bytes);
				player.LocalViewAngles2.GetBytes().AppendTo(ref bytes);
				// This is only in the first packet info but w/e
				// (is this actually right?)
				if (player.InSequence != null) ((int)player.InSequence).GetBytes().AppendTo(ref bytes);
				if (player.OutSequence != null) ((int)player.OutSequence).GetBytes().AppendTo(ref bytes);
			}
			RawData.GetBytes().AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> Infos.FirstOrDefault()?.ViewOrigin?.ToString() ?? "NULL";
	}
}