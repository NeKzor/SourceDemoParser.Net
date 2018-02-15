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
				
				if (AutoConfiguration)
					await Configure(demo).ConfigureAwait(false);
				
				if (Mode == ParsingMode.HeaderOnly)
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
					
					if ((Mode == ParsingMode.Everything) && (message.Frame != null))
						await message.Frame.Parse(demo).ConfigureAwait(false);

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
					demo.Game.HasAlignmentByte = false;
					demo.Game.DefaultMessages = DemoMessages.OldEngine;
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
					demo.Game.MaxSplitscreenClients = 2;
					demo.Game.DefaultNetMessages = NetMessages.Portal2;
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