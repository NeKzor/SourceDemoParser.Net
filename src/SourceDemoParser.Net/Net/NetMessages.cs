using System.Collections.Generic;
using SourceDemoParser.Types.Net;
using NetMessageTypes = System.Collections.Generic.List<SourceDemoParser.NetMessageType>;

namespace SourceDemoParser
{
	public static class NetMessages
	{
		public static NetMessageTypes Default = new NetMessageTypes()
		{
			new NetNop(0b0000_0000),
			new NetDisconnect(0b0000_0001),
			new NetFile(0b0000_0010),
			new NetTick(0b0000_0011),
			new NetStringCmd(0b0000_0100),
			new NetSetConVar(0b0000_0101),
			new NetSignonState(0b0000_0110),
			new SvcPrint(0b0000_0111),
			new SvcServerInfo(0b0000_1000),
			new SvcSendTable(0b0000_1001),
			new SvcClassInfo(0b0000_1010),
			new SvcSetPause(0b0000_1011),
			new SvcCreateStringTable(0b0000_1100),
			new SvcUpdateStringTable(0b0000_1101),
			new SvcVoiceInit(0b0000_1110),
			new SvcVoiceData(0b0000_1111),
			NetMessageType.Empty,
			new SvcSounds(0b0001_0001),
			new SvcSetView(0b0001_0010),
			new SvcFixAngle(0b0001_0011),
			new SvcCrosshairAngle(0b0001_0100),
			new SvcBspDecal(0b0001_0101),
			NetMessageType.Empty,
			new SvcUserMessage(0b0001_0111),
			new SvcEntityMessage(0b0001_1000),
			new SvcGameEvent(0b0001_1001),
			new SvcPacketEntities(0b0001_1010),
			new SvcTempEntities(0b0001_1011),
			new SvcPrefetch(0b0001_1100),
			new SvcMenu(0b0001_1101),
			new SvcGameEventList(0b0001_1110),
			new SvcGetCvarValue(0b0001_1111),
			new SvcCmdKeyValues(0b0010_0000)
		};
		public static NetMessageTypes Portal2 = new NetMessageTypes()
		{
			new NetNop(0b0000_0000),
			new NetDisconnect(0b0000_0001),
			new NetFile(0b0000_0010),
			new NetSplitScreenUser(0b0000_0011),
			new NetTick(0b0000_0100),
			new NetStringCmd(0b0000_0101),
			new NetSetConVar(0b0000_0110),
			new NetSignonState(0b0000_0111),
			new SvcServerInfo(0b0000_1000),
			new SvcSendTable(0b0000_1001),
			new SvcClassInfo(0b0000_1010),
			new SvcSetPause(0b0000_1011),
			new SvcCreateStringTable(0b0000_1100),
			new SvcUpdateStringTable(0b0000_1101),
			new SvcVoiceInit(0b0000_1110),
			new SvcVoiceData(0b0000_1111),
			new SvcPrint(0b0001_0000),
			new SvcSounds(0b0001_0001),
			new SvcSetView(0b0001_0010),
			new SvcFixAngle(0b0001_0011),
			new SvcCrosshairAngle(0b0001_0100),
			new SvcBspDecal(0b0001_0101),
			new SvcSplitScreen(0b0001_0110),
			new SvcUserMessage(0b0001_0111),
			new SvcEntityMessage(0b0001_1000),
			new SvcGameEvent(0b0001_1001),
			new SvcPacketEntities(0b0001_1010),
			new SvcTempEntities(0b0001_1011),
			new SvcPrefetch(0b0001_1100),
			new SvcMenu(0b0001_1101),
			new SvcGameEventList(0b0001_1110),
			new SvcGetCvarValue(0b0001_1111),
			new SvcCmdKeyValues(0b0010_0000)
		};
	}
}