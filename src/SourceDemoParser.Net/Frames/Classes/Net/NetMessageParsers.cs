using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public static class NetMessageParsers
	{
		public static Task<INetMessage> ParseNetNop(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(new NetNopMessage() as INetMessage);
		}
		public static Task<INetMessage> ParseNetDisconnect(ISourceBufferUtil buf, SourceDemo demo)
		{
			var reason = buf.ReadString();
			//System.Console.WriteLine("reason: " + reason);
			return Task.FromResult(new NetDisconnectMessage() { Reason = reason } as INetMessage);
		}
		public static Task<INetMessage> ParseNetFile(ISourceBufferUtil buf, SourceDemo demo)
		{
			var id = buf.ReadUInt32();
			var name = buf.ReadString();
			var requested = buf.ReadBoolean();
			return Task.FromResult(new NetFileMessage()
			{
				TransferId = id,
				FileName = name,
				FileRequested = requested
			} as INetMessage);
		}
		public static Task<INetMessage> ParseNetTick(ISourceBufferUtil buf, SourceDemo demo)
		{
			var id = buf.ReadInt32();
			var time = buf.ReadInt16();
			var timestd = buf.ReadInt16();
			System.Console.WriteLine(id);
			return Task.FromResult(new NetTickMessage()
			{
				Tick = id,
				HostFrameTime = time,
				HostFrameTimeStdDeviation = timestd
			} as INetMessage);
		}
		public static Task<INetMessage> ParseNetStringCmd(ISourceBufferUtil buf, SourceDemo demo)
		{
			var command = buf.ReadString(256); // 256 MAX_COMMAND_LEN
			return Task.FromResult(new NetStringCmdMessage() { Command = command } as INetMessage);
		}
		public static Task<INetMessage> ParseNetSetConvar(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt32();
			var convars = new List<ConVar>(length);
			while (length-- != 0)
			{
				convars.Add(new ConVar()
				{
					Name = buf.ReadString(),
					Value = buf.ReadString()
				});
			}
			return Task.FromResult(new NetSetConVarMessage() { ConVars = convars } as INetMessage);
		}
		public static Task<INetMessage> ParseNetSignonState(ISourceBufferUtil buf, SourceDemo demo)
		{
			var state = buf.ReadByte();
			var count = buf.ReadInt32();
			return Task.FromResult(new NetSignonStateMessage()
			{
				SignonState = state,
				SpawnCount = count
			} as INetMessage);
		}

		public static Task<INetMessage> ParseSvcPrint(ISourceBufferUtil buf, SourceDemo demo)
		{
			var text = buf.ReadString();
			return Task.FromResult(new SvcPrintMessage() { Text = text } as INetMessage);
		}
		public static Task<INetMessage> ParseSvcServerInfo(ISourceBufferUtil buf, SourceDemo demo)
		{
			var protocol = buf.ReadInt16();
			var count = buf.ReadInt32();
			var hltv = buf.ReadBoolean();
			var dedicated = buf.ReadBoolean();
			var client = buf.ReadInt32();
			var classes = buf.ReadUInt16();
			var mapcrc = (protocol < 18)
				? buf.ReadInt32()
				: buf.ReadBits(128);
			var slot = buf.ReadByte();
			var clients = buf.ReadByte();
			var tick = buf.ReadSingle();
			var os = buf.ReadChar();
			var dir = buf.ReadString();
			var map = buf.ReadString();
			var sky = buf.ReadString();
			var host = buf.ReadString();
			//var replay = buf.ReadBoolean();

			System.Console.WriteLine("protocol: " + protocol);
			System.Console.WriteLine("host: " + host);
			//System.Console.WriteLine("replay: " + replay);
			
			return Task.FromResult(new SvcServerInfoMessage()
			{
				Protocol = protocol,
				ServerCount = count,
				IsHltv = hltv,
				IsDedicated = dedicated,
				ClientCrc = client,
				MaxClasses = classes,
				MapCrc = mapcrc,
				PlayerSlot = slot,
				MaxClients = clients,
				TickInterval = tick,
				OperatingSystem = os,
				GameDir = dir,
				MapName = map,
				SkyName = sky,
				HostName = host
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcSendTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			var decoder = buf.ReadBoolean();
			var length = buf.ReadInt16();
			buf.SeekBits(length);
			//var data = buf.ReadBytes(length);
			return Task.FromResult(new SvcSendTableMessage()
			{
				NeedsDecoder = decoder,
				Length = length,
				//Data = data

			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcClassInfo(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt16();
			var create = buf.ReadBoolean();
			var servers = new List<ServerClassInfo>(length);
			if (!create)
			{
				while (length-- > 0)
				{
					servers.Add(new ServerClassInfo()
					{
						ClassId = (short)buf.ReadBits((int)System.Math.Log(length, 2) + 1),
						ClassName = buf.ReadString(),
						DataTableName = buf.ReadString()
					});
				}
			}
			return Task.FromResult(new SvcClassInfoMessage()
			{
				CreateOnClient = create,
				ServerClasses = servers
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcSetPause(ISourceBufferUtil buf, SourceDemo demo)
		{
			var paused = buf.ReadBoolean();
			return Task.FromResult(new SvcSetPauseMessage() { Paused = paused } as INetMessage);
		}
		public static Task<INetMessage> ParseSvcCreateStringTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			var name = buf.ReadString(); // 256
			System.Console.WriteLine(name);

			var max = buf.ReadInt16();
			System.Console.WriteLine(max);

			var toread = (int)System.Math.Log(max, 2) + 1;
			var entries = buf.ReadBits((toread == 1) ? 2 : toread);
			System.Console.WriteLine(entries);

			var length = buf.ReadBits(20); // NET_MAX_PALYLOAD_BITS + 3
			System.Console.WriteLine(length);

			var fixedsize = buf.ReadBoolean();
			System.Console.WriteLine(fixedsize);

			var size = (fixedsize) ? buf.ReadBits(12) : 0;
			var bits = (fixedsize) ? buf.ReadBits(4) : 0;
			var compressed = buf.ReadBoolean();
			System.Console.WriteLine(compressed);

			buf.SeekBits(length);
			//var data = buf.ReadBytes(length / 8);
			
			return Task.FromResult(new SvcCreateStringTableMessage()
			{
				TableName = name,
				MaxEntries = max,
				Entries = (int)entries,
				Length = length,
				UserDataFixedSize = fixedsize,
				UserDataSize = size,
				UserDataSizeBits = bits,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcUpdateStringTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			var id = buf.ReadBits(5);
			var changed = buf.ReadBoolean();
			var entries = (changed) ? buf.ReadInt16() : 1;
			var length = buf.ReadBits(20);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length);
			return Task.FromResult(new SvcUpdateStringTableMessage()
			{
				Id = id,
				EntriesChanged = changed,
				ChangedEntries = entries,
				Length = length,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcVoiceInit(ISourceBufferUtil buf, SourceDemo demo)
		{
			var codec = buf.ReadString(); // MAX_OSPATH
			var quality = buf.ReadByte();
			System.Console.WriteLine("codec: " + codec);
			System.Console.WriteLine("quality: " + quality);
			/* if (quality == (byte)255)
				_ = buf.ReadSingle(); */
			
			return Task.FromResult(new SvcVoiceInitMessage()
			{
				VoiceCodec = codec,
				Quality = quality
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcVoiceData(ISourceBufferUtil buf, SourceDemo demo)
		{
			var client = buf.ReadByte();
			var proximity = buf.ReadByte();
			var length = buf.ReadUInt16();
			var data = buf.ReadBytes(length);
			return Task.FromResult(new SvcVoiceDataMessage()
			{
				FromClient = client,
				Proximity = proximity,
				Length = length,
				Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcSounds(ISourceBufferUtil buf, SourceDemo demo)
		{
			var reliable = buf.ReadBoolean();
			var sounds = (reliable) ? 1 : buf.ReadBits(8);
			var length = (reliable) ? buf.ReadBits(8) : buf.ReadBits(16);
			buf.SeekBits(length);
			return Task.FromResult(new SvcSoundsMessage()
			{
				ReliableSound = reliable,
				Length = length,
				Sounds = sounds,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcSetView(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(11);
			return Task.FromResult(new SvcSetViewMessage() { EntityIndex = index } as INetMessage);
		}
		public static Task<INetMessage> ParseSvcFixAngle(ISourceBufferUtil buf, SourceDemo demo)
		{
			var relative = buf.ReadBoolean();
			var x = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			var y = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			var z = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			return Task.FromResult(new SvcFixAngleMessage()
			{
				Relative = relative,
				Angle = new QAngle(x, y, z)
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcCrosshairAngle(ISourceBufferUtil buf, SourceDemo demo)
		{
			var x = buf.ReadSingle();
			var y = buf.ReadSingle();
			var z = buf.ReadSingle();
			return Task.FromResult(new SvcCrosshairAngleMessage() { Angle = new QAngle(x, y, z) } as INetMessage);
		}
		public static Task<INetMessage> ParseSvcBspDecal(ISourceBufferUtil buf, SourceDemo demo)
		{
			var x = buf.ReadSingle();
			var y = buf.ReadSingle();
			var z = buf.ReadSingle();
			var texture = buf.ReadUInt32();
			var entities = buf.ReadBoolean();
			var entity = buf.ReadUInt32();
			var model = buf.ReadUInt32();
			var priority = buf.ReadBoolean();
			return Task.FromResult(new SvcBspDecalMessage()
			{
				Position = new Vector(x, y, z),
				DecalTextureIndex = texture,
				HasEntities = entities,
				EntityIndex = entity,
				ModelIndex = model,
				LowPriority = priority
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcUserMessage(ISourceBufferUtil buf, SourceDemo demo)
		{
			var type = buf.ReadBits(8);
			var length = buf.ReadBits(11);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length);
			return Task.FromResult(new SvcUserMessageMessage()
			{
				Length = length,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcEntityMessage(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(11); // MAX_EDICT_BITS
			var id = buf.ReadBits(9); // MAX_SERVER_CLASS_BITS
			var length = buf.ReadBits(11);
			System.Console.WriteLine(index);
			System.Console.WriteLine(id);
			System.Console.WriteLine(length);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length / 8);
			return Task.FromResult(new SvcEntityMessageMessage()
			{
				EntityIndex = index,
				ClassId = id,
				Length = length,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcGameEvent(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadBits(11); // ?
			var data = buf.ReadBytes(length);
			return Task.FromResult(new SvcGameEventMessage()
			{
				Length = length,
				Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcPacketEntities(ISourceBufferUtil buf, SourceDemo demo)
		{
			var max = buf.ReadBits(11); // ?
			var delta = buf.ReadBoolean();
			var from = (delta) ? buf.ReadInt32() : 0;
			//var length = buf.ReadUInt32();
			var baseline = buf.ReadBoolean();
			var entries = buf.ReadBits(11); // ?
			var length = buf.ReadBits(20);
			var update = buf.ReadBoolean();
			buf.SeekBits(length);
			return Task.FromResult(new SvcPacketEntitiesMessage()
			{
				MaxEntries = max,
				IsDelta = delta,
				DeltaFrom = from,
				BaseLine = baseline,
				UpdatedEntries = entries,
				Length = length,
				UpdateBaseline = update,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcTempEntities(ISourceBufferUtil buf, SourceDemo demo)
		{
			var entries = buf.ReadBits(8);
			var length = buf.ReadBits(17);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length);
			return Task.FromResult(new SvcTempEntitiesMessage()
			{
				Entries = entries,
				Length = length,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcPrefetch(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(13);
			return Task.FromResult(new SvcPrefetchMessage() { SoundIndex = index } as INetMessage);
		}
		public static Task<INetMessage> ParseSvcMenu(ISourceBufferUtil buf, SourceDemo demo)
		{
			var type = buf.ReadInt16();
			var length = buf.ReadUInt32();
			var data = buf.ReadBytes((int)length);
			return Task.FromResult(new SvcMenuMessage()
			{
				Type = type,
				Length = length,
				Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcGameEventList(ISourceBufferUtil buf, SourceDemo demo)
		{
			var events = buf.ReadBits(9); // ?
			var length = buf.ReadBits(20); // ?
			buf.SeekBits(length);
			//var data = buf.ReadBytes(length);
			return Task.FromResult(new SvcGameEventListMessage()
			{
				Events = events,
				Length = length,
				//Data = data
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcGetCvarValue(ISourceBufferUtil buf, SourceDemo demo)
		{
			var cookie = buf.ReadInt32();
			var name = buf.ReadString();
			return Task.FromResult(new SvcGetCvarValueMessage()
			{
				Cookie = cookie,
				CvarName = name
			} as INetMessage);
		}
		public static Task<INetMessage> ParseSvcCmdKeyValues(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadUInt32();
			var data = buf.ReadBytes((int)length);
			return Task.FromResult(new SvcCmdKeyValuesMessage()
			{
				Length = length,
				Data = data
			} as INetMessage);
		}
	}
}