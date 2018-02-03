using System.Collections.Generic;
using NetMessageTypes = System.Collections.Generic.List<SourceDemoParser.NetMessageType>;
using Parser = SourceDemoParser.NetMessageParsers;
using Exporter = SourceDemoParser.NetMessageExporters;

namespace SourceDemoParser
{
	public static class NetMessages
	{
		public static NetMessageTypes Default = new NetMessageTypes()
		{
			new NetMessageType("net_nop", Parser.ParseNetNop, Exporter.ExportNetNop),
			new NetMessageType("net_disconnect", Parser.ParseNetDisconnect, Exporter.ExportNetDisconnect),
			new NetMessageType("net_file", Parser.ParseNetFile, Exporter.ExportNetFile),
			new NetMessageType("net_tick", Parser.ParseNetTick, Exporter.ExportNetTick),
			new NetMessageType("net_stringcmd", Parser.ParseNetStringCmd, Exporter.ExportNetStringCmd),
			new NetMessageType("net_setconvar", Parser.ParseNetSetConvar, Exporter.ExportNetSetConvar),
			new NetMessageType("net_signonstate", Parser.ParseNetSignonState, Exporter.ExportNetSignonState),
			new NetMessageType("svc_print", Parser.ParseSvcPrint, Exporter.ExportSvcPrint),
			new NetMessageType("svc_serverinfo", Parser.ParseSvcServerInfo, Exporter.ExportSvcServerInfo),
			new NetMessageType("svc_sendtable", Parser.ParseSvcSendTable, Exporter.ExportSvcSendTable),
			new NetMessageType("svc_classinfo", Parser.ParseSvcClassInfo, Exporter.ExportSvcClassInfo),
			new NetMessageType("svc_setpause", Parser.ParseSvcSetPause, Exporter.ExportSvcSetPause),
			new NetMessageType("svc_createstringtable", Parser.ParseSvcCreateStringTable, Exporter.ExportSvcCreateStringTable),
			new NetMessageType("svc_updatestringtable", Parser.ParseSvcUpdateStringTable, Exporter.ExportSvcUpdateStringTable),
			new NetMessageType("svc_voiceinit", Parser.ParseSvcVoiceInit, Exporter.ExportSvcVoiceInit),
			new NetMessageType("svc_voicedata", Parser.ParseSvcVoiceData, Exporter.ExportSvcVoiceData),
			new NetMessageType(string.Empty, null, null),
			new NetMessageType("svc_sounds", Parser.ParseSvcSounds, Exporter.ExportSvcSounds),
			new NetMessageType("svc_setview", Parser.ParseSvcSetView, Exporter.ExportSvcSetView),
			new NetMessageType("svc_fixangle", Parser.ParseSvcFixAngle, Exporter.ExportSvcFixAngle),
			new NetMessageType("svc_crosshairangle", Parser.ParseSvcCrosshairAngle, Exporter.ExportSvcCrosshairAngle),
			new NetMessageType("svc_bspdecal", Parser.ParseSvcBspDecal, Exporter.ExportSvcBspDecal),
			new NetMessageType(string.Empty, null, null),
			new NetMessageType("svc_usermessage", Parser.ParseSvcUserMessage, Exporter.ExportSvcUserMessage),
			new NetMessageType("svc_entitymessage", Parser.ParseSvcEntityMessage, Exporter.ExportSvcEntityMessage),
			new NetMessageType("svc_gameevent", Parser.ParseSvcGameEvent, Exporter.ExportSvcGameEvent),
			new NetMessageType("svc_packetentities", Parser.ParseSvcPacketEntities, Exporter.ExportSvcPacketEntities),
			new NetMessageType("svc_tempentities", Parser.ParseSvcTempEntities, Exporter.ExportSvcTempEntities),
			new NetMessageType("svc_prefetch", Parser.ParseSvcPrefetch, Exporter.ExportSvcPrefetch),
			new NetMessageType("svc_menu", Parser.ParseSvcMenu, Exporter.ExportSvcMenu),
			new NetMessageType("svc_gameeventlist", Parser.ParseSvcGameEventList, Exporter.ExportSvcGameEventList),
			new NetMessageType("svc_getcvarvalue", Parser.ParseSvcGetCvarValue, Exporter.ExportSvcGetCvarValue),
			new NetMessageType("svc_cmdkeyvalues", Parser.ParseSvcCmdKeyValues, Exporter.ExportSvcCmdKeyValues)
		};
	}
}