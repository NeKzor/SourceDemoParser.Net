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
			bool autoConfiguration = true)
			: base(mode, autoAdjustment, autoConfiguration)
		{
		}

		public override async Task<SourceDemo> ParseAsync(Stream input)
		{
			var demo = default(SourceDemo);
			using (var br = new BinaryReader(input))
			{
				demo = await ParseHeader(br, new SourceDemo()).ConfigureAwait(false);

				if (Mode == ParsingMode.HeaderOnly)
					return demo;

				if (AutoConfiguration)
					await Configure(demo).ConfigureAwait(false);
				
				while (br.BaseStream.Position != br.BaseStream.Length)
				{
					var code = (int)br.ReadByte();
					var type = demo.GameMessages.ElementAtOrDefault(code - 1);
					if (type == null) throw new MessageTypeException(code, br.BaseStream.Position);

					if (type.Name == "Stop")
					{
						demo.Messages.Add(new DemoMessage{ Type = type.WithCode(code) });
						break;
					}

					var tick = br.ReadInt32();
					if (demo.HasAlignmentByte) br.ReadByte();

					var message = new DemoMessage()
					{
						Type = type.WithCode(code),
						CurrentTick = tick,
						Frame = await type.Handler.Invoke(br, demo).ConfigureAwait(false)
					};

					if ((Mode == ParsingMode.Everything) && (message.Frame != null))
						await message.Frame.ParseData(demo).ConfigureAwait(false);
					
					demo.Messages.Add(message);
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
		public override Task Configure(SourceDemo demo)
		{
			switch (demo.Protocol)
			{
				case 2:
				case 3:
					demo.HasAlignmentByte = false;
					demo.GameMessages = DemoMessages.OldEngine;
					break;
				case 4:
					break;
				default:
					throw new ProtocolException(demo.Protocol);
			}
			switch (demo.GameDirectory)
			{
				case "portal2":
				case "aperturetag":
				case "portal_stories":
				case "infra":
					demo.MaxSplitscreenClients = 2;
					break;
			}
			return Task.CompletedTask;
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