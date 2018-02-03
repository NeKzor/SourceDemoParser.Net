using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
			var reason = buf.ReadString(1024);
			return Task.FromResult(new NetDisconnectMessage() { Reason = reason } as INetMessage);
		}
		public static Task<INetMessage> ParseNetFile(ISourceBufferUtil buf, SourceDemo demo)
		{
			var id = buf.ReadUInt32();
			var name = buf.ReadString();
			var requested = buf.ReadOneBit() != 0;
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
			var time = buf.ReadUInt32();
			var timestd = buf.ReadUInt32();
			return Task.FromResult(new NetTickMessage()
			{
				Tick = id,
				HostFrameTime = time,
				HostFrameTimeStdDeviation = timestd
			} as INetMessage);
		}
		public static Task<INetMessage> ParseNetStringCmd(ISourceBufferUtil buf, SourceDemo demo)
		{
			var command = buf.ReadString();
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
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcServerInfo(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcSendTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcClassInfo(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcSetPause(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcCreateStringTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcUpdateStringTable(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcVoiceInit(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcVoiceData(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcSounds(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcSetView(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcFixAngle(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcCrosshairAngle(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcBspDecal(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcUserMessage(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcEntityMessage(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcGameEvent(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcPacketEntities(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcTempEntities(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcPrefetch(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcMenu(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcGameEventList(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcGetCvarValue(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
		public static Task<INetMessage> ParseSvcCmdKeyValues(ISourceBufferUtil buf, SourceDemo demo)
		{
			return Task.FromResult(default(INetMessage));
		}
	}
}