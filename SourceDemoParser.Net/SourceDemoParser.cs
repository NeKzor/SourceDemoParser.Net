using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Net.Handlers;

namespace SourceDemoParser.Net
{
	public partial class SourceDemo
	{
		public static async Task<SourceDemo> ParseFileAsync(string filePath)
		{
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				return await Parse(fs).ConfigureAwait(false);
		}
		public static async Task<SourceDemo> ParseContentAsync(byte[] demoContent)
		{
			using (var ms = new MemoryStream(demoContent))
				return await Parse(ms).ConfigureAwait(false);
		}
		public static Task<SourceDemo> Parse(Stream input)
		{
			var handler = default(BaseGameHandler);
			using (var br = new BinaryReader(input))
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

		public static Task<bool> TryParse(Stream input, out SourceDemo demo)
		{
			demo = default(SourceDemo);
			try
			{
				demo = Parse(input).ConfigureAwait(false).GetAwaiter().GetResult();
				return Task.FromResult(true);
			}
			catch
			{
			}
			return Task.FromResult(false);
		}
		public static Task<bool> TryParseFile(string filePath, out SourceDemo demo)
		{
			demo = default(SourceDemo);
			try
			{
				using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
					demo = Parse(fs).ConfigureAwait(false).GetAwaiter().GetResult();
				return Task.FromResult(true);
			}
			catch
			{
			}
			return Task.FromResult(false);
		}
		public static Task<bool> TryParseContent(byte[] demoContent, out SourceDemo demo)
		{
			demo = default(SourceDemo);
			try
			{
				using (var ms = new MemoryStream(demoContent))
					demo = Parse(ms).ConfigureAwait(false).GetAwaiter().GetResult();
				return Task.FromResult(true);
			}
			catch
			{
			}
			return Task.FromResult(false);
		}
	}
}