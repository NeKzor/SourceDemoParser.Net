using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SourceParser : SourceParserBase
	{
		public SourceParser(
			ParsingMode mode = default,
			AdjustmentType autoAdjustment = default,
			Func<SourceDemo, SourceGame> configBuilder = default)
			: base(mode, autoAdjustment, configBuilder)
		{
		}

		public override async Task<SourceDemo> ParseAsync(Stream input)
		{
			var demo = default(SourceDemo);
			using (var br = new BinaryReader(input))
			{
				demo = await ParseHeader(br, new SourceDemo()).ConfigureAwait(false);
				
				if (ConfigBuilder != null)
					await Task.Run(() => demo.Game = ConfigBuilder.Invoke(demo)).ConfigureAwait(false);
				
				if (!Mode.HasFlag(ParsingMode.Header))
					return demo;

				while (br.BaseStream.Position != br.BaseStream.Length)
				{
					var type = (int)br.ReadByte();
					var message = demo.Game.DefaultMessages
						.ElementAtOrDefault(type - 1)?.GetMessage();
					
					if (message == null)
						throw new MessageTypeException(type, br.BaseStream.Position);

					if (message.Type.Name == "Stop")
					{
						message.Tick = demo.Messages.Last().Tick;
						await message.Parse(br, demo).ConfigureAwait(false);
						demo.Messages.Add(message);
						break;
					}

					message.Tick = br.ReadInt32();
					if (demo.Game.HasAlignmentByte) br.ReadByte();

					await message.Parse(br, demo).ConfigureAwait(false);
					demo.Messages.Add(message);
				}

				if (Mode == ParsingMode.Everything)
					await demo.ParseAllFrames().ConfigureAwait(false);
				else
				{
					if (Mode.HasFlag(ParsingMode.ConsoleCmd))
						await demo.GetMessagesByType("ConsoleCmd").ParseFrames(demo).ConfigureAwait(false);
					if (Mode.HasFlag(ParsingMode.CustomData))
						await demo.GetMessagesByType("CustomData").ParseFrames(demo).ConfigureAwait(false);
					if (Mode.HasFlag(ParsingMode.DataTables))
						await demo.GetMessagesByType("DataTables").ParseFrames(demo).ConfigureAwait(false);
					if (Mode.HasFlag(ParsingMode.Packet))
						await demo.GetMessagesByType("Packet").ParseFrames(demo).ConfigureAwait(false);
					if (Mode.HasFlag(ParsingMode.StringTables))
						await demo.GetMessagesByType("StringTables").ParseFrames(demo).ConfigureAwait(false);
					if (Mode.HasFlag(ParsingMode.UserCmd))
						await demo.GetMessagesByType("UserCmd").ParseFrames(demo).ConfigureAwait(false);
				}
				
				if (AutoAdjustment == AdjustmentType.Exact)
					await demo.AdjustExact().ConfigureAwait(false);
			}
			return demo;
		}
		public override Task<SourceDemo> ParseHeader(BinaryReader br, SourceDemo demo)
		{
			// DEMO_HEADER_ID
			demo.HeaderId = Encoding.ASCII.GetString(br.ReadBytes(8));
			if (demo.HeaderId != "HL2DEMO\0")
				throw new SourceException(demo.HeaderId);

			// DEMO_PROTOCOL
			demo.Protocol = br.ReadInt32();
			demo.NetworkProtocol = br.ReadInt32();
			demo.ServerName = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
			demo.ClientName = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
			demo.MapName = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
			demo.GameDirectory = Encoding.ASCII.GetString(br.ReadBytes(260)).TrimEnd(new char[1]);
			demo.PlaybackTime = br.ReadSingle();
			demo.PlaybackTicks = br.ReadInt32();
			demo.PlaybackFrames = br.ReadInt32();
			demo.SignOnLength = br.ReadInt32();
			demo.Messages = new List<IDemoMessage>();

			return Task.FromResult(demo);
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