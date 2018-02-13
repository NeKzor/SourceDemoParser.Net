using System.Collections.Generic;
using SourceDemoParser.Types.Net;
using NetMessageTypes = System.Collections.Generic.List<SourceDemoParser.NetMessageType>;

namespace SourceDemoParser
{
	public static class NetMessages
	{
		public static NetMessageTypes Default = new NetMessageTypes()
		{
			new NetNop(),
			new NetDisconnect(),
			new NetFile(),
			new NetTick(),
			new NetStringCmd(),
			new NetSetConVar(),
			new NetSignonState(),
			new SvcPrint(),
			new SvcServerInfo(),
			new SvcSendTable(),
			new SvcClassInfo(),
			new SvcSetPause(),
			new SvcCreateStringTable(),
			new SvcUpdateStringTable(),
			new SvcVoiceInit(),
			new SvcVoiceData(),
			NetMessageType.Empty,
			new SvcSounds(),
			new SvcSetView(),
			new SvcFixAngle(),
			new SvcCrosshairAngle(),
			new SvcBspDecal(),
			NetMessageType.Empty,
			new SvcUserMessage(),
			new SvcEntityMessage(),
			new SvcGameEvent(),
			new SvcPacketEntities(),
			new SvcTempEntities(),
			new SvcPrefetch(),
			new SvcMenu(),
			new SvcGameEventList(),
			new SvcGetCvarValue(),
			new SvcCmdKeyValues()
		};
		public static NetMessageTypes Portal2 = new NetMessageTypes()
		{
			new NetNop(),
			new NetDisconnect(),
			new NetFile(),
			new NetSplitScreenUser(),
			new NetTick(),
			new NetStringCmd(),
			new NetSetConVar(),
			new NetSignonState(),
			new SvcServerInfo(),
			new SvcSendTable(),
			new SvcClassInfo(),
			new SvcSetPause(),
			new SvcCreateStringTable(),
			new SvcUpdateStringTable(),
			new SvcVoiceInit(),
			new SvcVoiceData(),
			new SvcPrint(),
			new SvcSounds(),
			new SvcSetView(),
			new SvcFixAngle(),
			new SvcCrosshairAngle(),
			new SvcBspDecal(),
			new SvcSplitScreen(),
			new SvcUserMessage(),
			new SvcEntityMessage(),
			new SvcGameEvent(),
			new SvcPacketEntities(),
			new SvcTempEntities(),
			new SvcPrefetch(),
			new SvcMenu(),
			new SvcGameEventList(),
			new SvcGetCvarValue(),
			new SvcCmdKeyValues()
		};
	}
}