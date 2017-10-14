using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser.Extensions
{
	public static class SourceExporter
	{
		// TODO: Test this
		public static async Task<byte[]> ExportContentAsync(this SourceDemo demo, bool headerOnly = false, bool fastExport = false)
		{
			var result = default(byte[]);
			using (var ms = new MemoryStream())
			using (var bw = new BinaryWriter(ms, Encoding.ASCII))
			{
				await bw.InternalExportAsync(demo, headerOnly, fastExport).ConfigureAwait(false);
				result = ms.ToArray();
			}
			return result;
		}

		public static async Task ExportFileAsync(this SourceDemo demo, string filePath, FileMode mode = FileMode.Create, bool headerOnly = false, bool fastExport = false)
		{
			using (var fs = new FileStream(filePath, mode, FileAccess.Write))
			using (var bw = new BinaryWriter(fs, Encoding.ASCII))
			{
				await bw.InternalExportAsync(demo, headerOnly, fastExport).ConfigureAwait(false);
			}
		}

		internal static async Task InternalExportAsync(this BinaryWriter bw, SourceDemo demo, bool headerOnly = false, bool fastExport = false)
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

				if (message.Type == DemoMessageType.SyncTick)
					continue;

				var bytes = default(byte[]);
				switch (message.Type)
				{
					case DemoMessageType.SignOn:
					case DemoMessageType.Packet:
						if (fastExport) bytes = await InternalExporter.ExportPacket(message.Frame as PacketFrame).ConfigureAwait(false);
						break;
					case DemoMessageType.ConsoleCmd:
						if (fastExport) bytes = await InternalExporter.ExportConsoleCmd(message.Frame as ConsoleCmdFrame).ConfigureAwait(false);
						break;
					case DemoMessageType.UserCmd:
						if (fastExport) bytes = await InternalExporter.ExportUserCmd(message.Frame as UserCmdFrame).ConfigureAwait(false);
						break;
					case DemoMessageType.DataTables:
						if (fastExport) bytes = await InternalExporter.ExportDataTables(message.Frame as DataTablesFrame).ConfigureAwait(false);
						break;
					case DemoMessageType.CustomData:
						if (fastExport) bytes = await InternalExporter.ExportCustomData(message.Frame as CustomDataFrame).ConfigureAwait(false);
						break;
					case DemoMessageType.StringTables:
						if (demo.Protocol != 4)
							throw new MessageTypeException(message.CurrentTick, message.Type);
						if (fastExport) bytes = await InternalExporter.ExportStringTables(message.Frame as StringTablesFrame).ConfigureAwait(false);
						break;
					default:
						throw new MessageTypeException(message.CurrentTick, message.Type);
				}

				if (!fastExport)
					bytes = await message.Frame.ExportData().ConfigureAwait(false);
				if (bytes != null)
					bw.Write(bytes);
			}
		}
	}
}