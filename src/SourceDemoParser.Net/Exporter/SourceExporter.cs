using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class SourceExporter : SourceExporterBase
	{
		public SourceExporter(ExportMode mode = default)
			: base(mode)
		{
		}

		public override async Task ExportAsync(BinaryWriter bw, SourceDemo demo)
		{
			// DEMO_HEADER_ID
			if (demo.HeaderId != "HL2DEMO\0")
				throw new SourceException(demo.HeaderId);
			bw.Write(demo.HeaderId.ToCharArray());

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

			if (Mode == ExportMode.HeaderOnly)
				return;

			foreach (var message in demo.Messages)
			{
				bw.Write((byte)message.Type);
				bw.Write(message.CurrentTick);

				if (message.Type == DemoMessageType.Stop)
					break;
				
				bw.Write(0x00);

				if (message.Type == DemoMessageType.SyncTick)
					continue;
				
				if (message.Frame == null)
					continue;
				
				var data = default(byte[]);
				if (Mode == ExportMode.FrameData)
					data = await message.Frame.ExportData().ConfigureAwait(false);
				else
					data = await HandleMessageAsync(message).ConfigureAwait(false);
				
				bw.Write(data);
			}
		}
		public override async Task<byte[]> HandleMessageAsync(IDemoMessage message)
		{
			var data = default(byte[]);
			switch (message.Type)
			{
				case DemoMessageType.SignOn:
				case DemoMessageType.Packet:
					data = await ExportPacket(message.Frame as PacketFrame).ConfigureAwait(false);
					break;
				case DemoMessageType.ConsoleCmd:
					data = await ExportConsoleCmd(message.Frame as ConsoleCmdFrame).ConfigureAwait(false);
					break;
				case DemoMessageType.UserCmd:
					data = await ExportUserCmd(message.Frame as UserCmdFrame).ConfigureAwait(false);
					break;
				case DemoMessageType.DataTables:
					data = await ExportDataTables(message.Frame as DataTablesFrame).ConfigureAwait(false);
					break;
				case DemoMessageType.CustomData:
					data = await ExportCustomData(message.Frame as CustomDataFrame).ConfigureAwait(false);
					break;
				case DemoMessageType.StringTables:
					data = await ExportStringTables(message.Frame as StringTablesFrame).ConfigureAwait(false);
					break;
				default:
					throw new MessageTypeException(message);
			}
			return data;
		}

		public async Task<byte[]> ExportContentAsync(SourceDemo demo)
		{
			var result = default(byte[]);
			using (var ms = new MemoryStream())
			using (var bw = new BinaryWriter(ms, Encoding.ASCII))
			{
				await ExportAsync(bw, demo).ConfigureAwait(false);
				result = ms.ToArray();
			}
			return result;
		}
		public async Task ExportFileAsync(SourceDemo demo, string filePath, FileMode fileMode = FileMode.Create)
		{
			using (var fs = new FileStream(filePath, fileMode, FileAccess.Write))
			using (var bw = new BinaryWriter(fs, Encoding.ASCII))
			{
				await ExportAsync(bw, demo).ConfigureAwait(false);
			}
		}
	}
}