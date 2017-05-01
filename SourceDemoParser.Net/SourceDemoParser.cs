using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Net.Handlers;

namespace SourceDemoParser.Net
{
	public partial class SourceDemo
	{
		public static Task<SourceDemo> Parse(string filePath)
		{
			var handler = default(BaseGameHandler);
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (var br = new BinaryReader(fs))
			{
				if (Encoding.ASCII.GetString(br.ReadBytes(8)).TrimEnd(new char[1]) != "HL2DEMO")
					throw new Exception("Not a demo!");

				var protocol = (SourceDemoProtocolVersion)br.ReadInt32();
				if ((int)protocol == 2)
					protocol = SourceDemoProtocolVersion.HL2;

				if (!(Enum.IsDefined(typeof(SourceDemoProtocolVersion), protocol)))
					throw new Exception($"Unknown demo protocol version: 0x{protocol.ToString("X")}");

				var netproc = br.ReadInt32();
				var server = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
				var client = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
				var map = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
				var dir = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
				var time = br.ReadSingle();
				var ticks = br.ReadInt32();
				var frames = br.ReadInt32();
				var signon = br.ReadInt32();

				handler = BaseGameHandler.GetGameHandler(dir, map, protocol);
				handler.FilePath = filePath;
				handler.NetworkProtocol = netproc;
				handler.Server = server;
				handler.Client = client;
				handler.MapName = map;
				handler.GameDirectory = dir;
				handler.Time = time;
				handler.TotalTicks = ticks;
				handler.FrameCount = frames;
				handler.SignOnLength = signon;

				var cmd = default(byte);
				while (!(handler.IsStop(cmd = br.ReadByte())))
				{
					var tick = br.ReadInt32();
					if (handler.DemoProtocol >= SourceDemoProtocolVersion.ORANGEBOX)
						br.ReadByte();
					handler.HandleCommand(cmd, tick, br);
				}
			}
			return Task.FromResult(handler.GetResult());
		}

		public static Task<bool> TryParse(string filePath, out SourceDemo demo)
		{
			demo = default(SourceDemo);
			try
			{
				demo = Parse(filePath).GetAwaiter().GetResult();
				return Task.FromResult(true);
			}
			catch
			{
			}
			return Task.FromResult(false);
		}
	}
}