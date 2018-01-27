using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SourceParser : SourceParserBase
	{
		public SourceParser(ParsingMode mode = default, AdjustmentType autoAdjustment = default)
			: base(mode, autoAdjustment)
		{
		}

		public override async Task<SourceDemo> ParseAsync(Stream input)
		{
			var demo = new SourceDemo();
			using (var br = new BinaryReader(input))
			{
				// DEMO_HEADER_ID
				demo.HeaderId = Encoding.ASCII.GetString(br.ReadBytes(8));
				if (demo.HeaderId != "HL2DEMO\0")
					throw new SourceException(demo.HeaderId);

				// DEMO_PROTOCOL
				demo.Protocol = br.ReadInt32();
				if (demo.Protocol != 4)
					throw new ProtocolException(demo.Protocol);

				demo.NetworkProtocol = br.ReadInt32();

				demo.ServerName = Encoding.ASCII
					.GetString(br.ReadBytes(260))
					.TrimEnd(new char[1]);
				demo.ClientName = Encoding.ASCII
					.GetString(br.ReadBytes(260))
					.TrimEnd(new char[1]);
				demo.MapName = Encoding.ASCII
					.GetString(br.ReadBytes(260))
					.TrimEnd(new char[1]);
				demo.GameDirectory = Encoding.ASCII
					.GetString(br.ReadBytes(260))
					.TrimEnd(new char[1]);
				
				demo.PlaybackTime = br.ReadSingle();
				demo.PlaybackTicks = br.ReadInt32();
				demo.PlaybackFrames = br.ReadInt32();

				demo.SignOnLength = br.ReadInt32();
				demo.Messages = new List<IDemoMessage>();

				if (Mode == ParsingMode.HeaderOnly)
					return demo;
				
				var message = default(DemoMessage);
				var type = default(DemoMessageType);
				var tick = default(int);
				var frame = default(IFrame);

				while (br.BaseStream.Position != br.BaseStream.Length)
				{
					type = (DemoMessageType)br.ReadByte();
					if (type == DemoMessageType.Stop)
					{
						demo.Messages.Add(new DemoMessage{ Type = type });
						break;
					}

					tick = br.ReadInt32();
					br.ReadByte();
					frame = await HandleMessageAsync(br, message = new DemoMessage(type, tick)).ConfigureAwait(false);

					if (frame != null)
					{
						if (Mode == ParsingMode.Everything)
							await frame.ParseData(demo).ConfigureAwait(false);
						message.Frame = frame;
					}

					demo.Messages.Add(message);
				}

				if (AutoAdjustment == AdjustmentType.Exact)
					await demo.AdjustExact().ConfigureAwait(false);
			}
			return demo;
		}
		public override async Task<IFrame> HandleMessageAsync(BinaryReader br, IDemoMessage message)
		{
			var frame = default(IFrame);
			switch (message.Type)
			{
				case DemoMessageType.SignOn:
				case DemoMessageType.Packet:
					frame = await ParsePacketAsync(br).ConfigureAwait(false);
					break;
				case DemoMessageType.SyncTick:
					break;
				case DemoMessageType.ConsoleCmd:
					frame = await ParseConsoleCmdAsync(br).ConfigureAwait(false);
					break;
				case DemoMessageType.UserCmd:
					frame = await ParseUserCmdAsync(br).ConfigureAwait(false);
					break;
				case DemoMessageType.DataTables:
					frame = await ParseDataTablesAsync(br).ConfigureAwait(false);
					break;
				case DemoMessageType.CustomData:
					frame = await ParseCustomDataAsync(br).ConfigureAwait(false);
					break;
				case DemoMessageType.StringTables:
					frame = await ParseStringTablesAsync(br).ConfigureAwait(false);
					break;
				default:
					throw new MessageTypeException(message);
			}
			return frame;
		}

		public async Task<SourceDemo> ParseFileAsync(string filePath)
		{
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				return await ParseAsync(fs).ConfigureAwait(false);
		}
		public async Task<SourceDemo> ParseContentAsync(byte[] demoContent)
		{
			using (var ms = new MemoryStream(demoContent))
				return await ParseAsync(ms).ConfigureAwait(false);
		}
	}
}