using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	public static class SourceExporter
	{
		public static async Task ExportFileAsync(this SourceDemo demo, string filePath, FileMode mode = FileMode.Create, bool headerOnly = false)
		{
			using (var fs = new FileStream(filePath, mode, FileAccess.Write))
			using (var bw = new BinaryWriter(fs, Encoding.ASCII))
			{
				// DEMO_HEADER_ID
				if (demo.FileStamp != "HL2DEMO\0")
					throw new SourceException(demo.FileStamp);
				bw.Write(demo.FileStamp.ToCharArray());

				// DEMO_PROTOCOL
				bw.Write(demo.Protocol);
				bw.Write(demo.NetworkProtocol);
				// MAX_OSPATH (260)
				bw.Write(Encoding.ASCII.GetBytes($"{demo.ServerName}\0").ToBuffer(260));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.ClientName}\0").ToBuffer(260));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.MapName}\0").ToBuffer(260));
				bw.Write(Encoding.ASCII.GetBytes($"{demo.GameDirectory}\0").ToBuffer(260));
				bw.Write(demo.PlaybackTime);
				bw.Write(demo.PlaybackTicks);
				bw.Write(demo.PlaybackFrames);
				bw.Write(demo.SignOnLength);

				if (headerOnly)
					return;

				foreach (var message in demo.Messages)
				{
					bw.Write((byte)message.Type);
					bw.Write(message.CurrentTick);

					if (message.Type == DemoMessageType.Stop)
						break;

					if (message.Tag != null)    // demo.Protocol == 4
						bw.Write((byte)message.Tag);

					switch (message.Type)
					{
						case DemoMessageType.SignOn:
							if (demo.SignOnData.Length == demo.SignOnLength)
								bw.Write(demo.SignOnData);
							else
								throw new Exception("SignOnLength doesn't match real length.");
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

					var bytes = await (message.Frame as IFrame).ExportData().ConfigureAwait(false);
					if (bytes != null)
						bw.Write(bytes);
				}
			}
		}

		// Helper
		private static byte[] ToBuffer(this byte[] data, int length)
		{
			var buffer = new byte[length];
			for (int i = 0; i < data.Length; i++)
				buffer[i] = data[i];
			return buffer;
		}
	}
}