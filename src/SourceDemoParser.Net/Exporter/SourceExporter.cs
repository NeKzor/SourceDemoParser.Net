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
			await ExportHeader(bw, demo).ConfigureAwait(false);
			if (Mode == ExportMode.HeaderOnly)
				return;

			foreach (var message in demo.Messages)
			{
				bw.Write((byte)message.Type);
				bw.Write(message.Tick);

				if (message.Type == 0x07)
					break;
				
				if (demo.Game.HasAlignmentByte) bw.Write(0x00);
				
				if (message.Frame == null)
					continue;
				
				if (Mode == ExportMode.FrameData)
				{
					var data = await message.Frame.Export().ConfigureAwait(false);
					if (data != null) bw.Write(data);
					continue;
				}

				await demo.Game.DefaultMessages[message.Type - 1]
					.GetMessage()
					.Export(bw, demo).ConfigureAwait(false);
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