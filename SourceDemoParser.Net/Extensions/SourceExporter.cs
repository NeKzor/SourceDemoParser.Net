using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Net;

namespace SourceDemoParser.Net.Extensions
{
	public static class SourceExporter
	{
		public static async Task ExportFileAsync(this SourceDemo demo, string filePath, FileMode mode = FileMode.Create, bool headerOnly = false)
		{
			using (var fs = new FileStream(filePath, mode, FileAccess.Write))
			using (var ds = await demo.ExportAsync(headerOnly).ConfigureAwait(false))
				ds.CopyTo(fs);
			//return Task.FromResult(0);
		}
		//public static async Task<IEnumerable<byte>> ExportContentAsync(this SourceDemo demo, bool headerOnly = false)
		//{
		//	using (var ds = await demo.ExportAsync(headerOnly).ConfigureAwait(false))
		//		return ds.ToArray();
		//}
		public static async Task<Stream> ExportAsync(this SourceDemo demo, bool headerOnly = false)
		{
			using (var ms = new MemoryStream())
			using (var bw = new BinaryWriter(ms, Encoding.ASCII))
			{
				// DEMO_HEADER_ID
				if (demo.FileStamp != "HL2DEMO\0")
					throw new SourceException(demo.FileStamp);
				bw.Write(demo.FileStamp.ToCharArray());
				
				// DEMO_PROTOCOL
				bw.Write(demo.Protocol);
				bw.Write(demo.NetworkProtocol);
				// MAX_OSPATH (260)
				var buffer = new byte[260];
				bw.Write(Encoding.ASCII.GetBytes($"{demo.ServerName}\0", 0, demo.ServerName.Length + 1, buffer, 0));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.ClientName}\0", 0, demo.ClientName.Length + 1, buffer, 0));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.MapName}\0", 0, demo.MapName.Length + 1, buffer, 0));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.GameDirectory}\0", 0, demo.GameDirectory.Length + 1, buffer, 0));
				bw.Write(demo.PlaybackTime);
				bw.Write(demo.PlaybackTicks);
				bw.Write(demo.PlaybackFrames);
				bw.Write(demo.SignOnLength);
				
				if (headerOnly)
					return ms;
				
				foreach (var message in demo.Messages)
				{
					bw.Write((byte)message.Type);
					bw.Write(message.CurrentTick);
					
					if (message.Type == DemoMessageType.Stop)
						break;
					
					if (message.Optional != null)	// == 4
						bw.Write((byte)message.Optional);
					
					var bytes = default(byte[]);
					switch (message.Type)
					{
						case DemoMessageType.SignOn:
							// Todo
							continue;
						case DemoMessageType.Packet:
							break;
						case DemoMessageType.SyncTick:
							//
							continue;
						case DemoMessageType.ConsoleCmd:
							break;
						case DemoMessageType.UserCmd:
							break;
						case DemoMessageType.DataTables:
							break;
						//case DemoMessageType.Stop:
							//break;
						case DemoMessageType.CustomData:
							break;
						case DemoMessageType.StringTables:
							if (demo.Protocol != 4)
								throw new MessageTypeException(message.CurrentTick, message.Type);
							break;
						default:
							throw new MessageTypeException(message.CurrentTick, message.Type);
					}
					bytes = await (message.Frame as IFrame).ExportData().ConfigureAwait(false);
					if (bytes != null)
						bw.Write(bytes);
				}
				return ms;
			}
		}
	}
}