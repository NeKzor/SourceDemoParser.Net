using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SourceParser
	{
		public bool HeaderOnly { get; set; }
		public bool FastParsing { get; set; }
		public bool AutoAdjustment { get; set; }
		public bool AutoGameDetection { get; set; }
		public event Func<object, DemoMessage, Task> OnDemoMessage;
		public event Func<object, DemoMessage, Task> OnSignOn;
		public event Func<object, DemoMessage, Task> OnPacket;
		public event Func<object, DemoMessage, Task> OnSyncTick;
		public event Func<object, DemoMessage, Task> OnConsoleCmd;
		public event Func<object, DemoMessage, Task> OnUserCmd;
		public event Func<object, DemoMessage, Task> OnDataTables;
		public event Func<object, DemoMessage, Task> OnStop;
		public event Func<object, DemoMessage, Task> OnCustomData;
		public event Func<object, DemoMessage, Task> OnStringTables;

		public SourceParser(
			bool headerOnly = false,
			bool fastParsing = false,
			bool autoAdjustment = false,
			bool autoGameDetection = true)
		{
			HeaderOnly = headerOnly;
			FastParsing = fastParsing;
			AutoAdjustment = autoAdjustment;
			AutoGameDetection = autoGameDetection;
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
		public async Task<SourceDemo> ParseAsync(Stream input)
		{
			var demo = new SourceDemo();
			using (var br = new BinaryReader(input))
			{
				// DEMO_HEADER_ID
				demo.FileStamp = Encoding.ASCII.GetString(br.ReadBytes(8));
				if (demo.FileStamp != "HL2DEMO\0")
					throw new SourceException(demo.FileStamp);

				// DEMO_PROTOCOL
				demo.Protocol = br.ReadInt32();
				if ((demo.Protocol > 4) || (demo.Protocol < 2))
					throw new ProtocolException(demo.Protocol);

				demo.NetworkProtocol = br.ReadInt32();
				demo.ServerName = Encoding.ASCII.GetString(br.ReadBytes(Const.MAX_OSPATH)).TrimEnd(new char[1]);
				demo.ClientName = Encoding.ASCII.GetString(br.ReadBytes(Const.MAX_OSPATH)).TrimEnd(new char[1]);
				demo.MapName = Encoding.ASCII.GetString(br.ReadBytes(Const.MAX_OSPATH)).TrimEnd(new char[1]);
				demo.GameDirectory = Encoding.ASCII.GetString(br.ReadBytes(Const.MAX_OSPATH)).TrimEnd(new char[1]);
				demo.PlaybackTime = br.ReadSingle();
				demo.PlaybackTicks = br.ReadInt32();
				demo.PlaybackFrames = br.ReadInt32();
				demo.SignOnLength = br.ReadInt32();
				demo.Messages = new List<IDemoMessage>();

				if (HeaderOnly)
					return demo;

				if (AutoGameDetection)
					demo.ConfigureParser();

				var message = default(DemoMessage);
				var type = default(DemoMessageType);
				var tick = default(int);
				var tag = default(byte?);
				var frame = default(IFrame);

				while (br.BaseStream.Position != br.BaseStream.Length)
				{
					type = (DemoMessageType)br.ReadByte();
					if (type == DemoMessageType.Stop)
						break;

					tick = br.ReadInt32();
					tag = (demo.Protocol == 4) ? br.ReadByte() : default(byte?);
					frame = default(IFrame);

					var e = default(Func<object, DemoMessage, Task>);
					switch (type)
					{
						case DemoMessageType.SignOn:
							frame = await InternalParser.ProcessPacket(br).ConfigureAwait(false);
							e = OnSignOn;
							break;
						case DemoMessageType.Packet:
							frame = await InternalParser.ProcessPacket(br).ConfigureAwait(false);
							e = OnPacket;
							break;
						case DemoMessageType.SyncTick:
							e = OnSyncTick;
							break;
						case DemoMessageType.ConsoleCmd:
							frame = await InternalParser.ProcessConsoleCmd(br).ConfigureAwait(false);
							e = OnConsoleCmd;
							break;
						case DemoMessageType.UserCmd:
							frame = await InternalParser.ProcessUserCmd(br).ConfigureAwait(false);
							e = OnUserCmd;
							break;
						case DemoMessageType.DataTables:
							frame = await InternalParser.ProcessDataTables(br).ConfigureAwait(false);
							e = OnDataTables;
							break;
						case DemoMessageType.CustomData:
							if (demo.Protocol != 4)
							{
								frame = await InternalParser.ProcessStringTables(br).ConfigureAwait(false);
								e = OnStringTables;
							}
							else
							{
								frame = await InternalParser.ProcessCustomData(br).ConfigureAwait(false);
								e = OnCustomData;
							}
							break;
						case DemoMessageType.StringTables:
							if (demo.Protocol != 4)
								throw new MessageTypeException(tick, type);
							frame = await InternalParser.ProcessStringTables(br).ConfigureAwait(false);
							e = OnStringTables;
							break;
						default:
							throw new MessageTypeException(tick, type);
					}

					// Parse data frame
					if ((frame != null) && (!FastParsing))
						await frame.ParseData().ConfigureAwait(false);

					message = new DemoMessage(type, tick, tag, frame);
					demo.Messages.Add(message);

					// Send events
					if (e != null) await e.Invoke(this, message).ConfigureAwait(false);
					if (OnDemoMessage != null) await OnDemoMessage.Invoke(this, message).ConfigureAwait(false);
				}
				// Add last demo message too
				message = new DemoMessage(type, tick, null, null);
				demo.Messages.Add(message);
				if (OnStop != null) await OnStop.Invoke(this, message).ConfigureAwait(false);

				if (AutoAdjustment)
					await demo.AdjustExact().ConfigureAwait(false);
			}
			return demo;
		}
	}
}