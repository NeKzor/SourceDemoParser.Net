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
			await ExportHeader(bw, demo);
			if (Mode == ExportMode.HeaderOnly)
				return;

			foreach (var message in demo.Messages)
			{
				var code = (byte)message.Type.Code;
				bw.Write(code);
				bw.Write(message.CurrentTick);

				if (message.Type.Name == "Stop")
					break;
				
				if (demo.HasAlignmentByte) bw.Write(0x00);
				
				if (message.Frame == null)
					continue;
				
				if (Mode == ExportMode.FrameData)
				{
					var data = await message.Frame.ExportData().ConfigureAwait(false);
					if (data != null) bw.Write(data);
					continue;
				}
				
				await demo.GameMessages[code - 1].Exporter.Invoke(bw, message.Frame).ConfigureAwait(false);
			}
		}
		public override Task ExportHeader(BinaryWriter bw, SourceDemo demo)
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

			return Task.CompletedTask;
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