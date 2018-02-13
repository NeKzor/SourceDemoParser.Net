using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class NetNop : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetNopMessage();
	}
	public class NetDisconnect : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetDisconnectMessage();
	}
	public class NetFile : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetFileMessage();
	}
	public class NetTick : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetTickMessage();
	}
	public class NetStringCmd : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetStringCmdMessage();
	}
	public class NetSetConVar : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetSetConVarMessage();
	}
	public class NetSignonState : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetSignonStateMessage();
	}
	public class SvcPrint : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcPrintMessage();
	}
	public class SvcServerInfo : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcServerInfoMessage();
	}
	public class SvcSendTable : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcSendTableMessage();
	}
	public class SvcClassInfo : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcClassInfoMessage();
	}
	public class SvcSetPause : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcSetPauseMessage();
	}
	public class SvcCreateStringTable : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcCreateStringTableMessage();
	}
	public class SvcUpdateStringTable : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcUpdateStringTableMessage();
	}
	public class SvcVoiceInit : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcVoiceInitMessage();
	}
	public class SvcVoiceData : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcVoiceDataMessage();
	}
	public class SvcSounds : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcSoundsMessage();
	}
	public class SvcSetView : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcSetViewMessage();
	}
	public class SvcFixAngle : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcFixAngleMessage();
	}
	public class SvcCrosshairAngle : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcCrosshairAngleMessage();
	}
	public class SvcBspDecal : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcBspDecalMessage();
	}
	public class SvcUserMessage : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcUserMessageMessage();
	}
	public class SvcEntityMessage : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcEntityMessageMessage();
	}
	public class SvcGameEvent : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcGameEventMessage();
	}
	public class SvcPacketEntities : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcPacketEntitiesMessage();
	}
	public class SvcTempEntities : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcTempEntitiesMessage();
	}
	public class SvcPrefetch : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcPrefetchMessage();
	}
	public class SvcMenu : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcMenuMessage();
	}
	public class SvcGameEventList : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcGameEventListMessage();
	}
	public class SvcGetCvarValue : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcGetCvarValueMessage();
	}
	public class SvcCmdKeyValues : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcCmdKeyValuesMessage();
	}
	public class NetSplitScreenUser : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new NetSplitScreenUserMessage();
	}
	public class SvcSplitScreen : NetMessageType
	{
		public override INetMessage GetMessage()
			=> new SvcSplitScreenMessage();
	}
}