using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public static class NetMessageExporters
	{
		public static Task ExportNetNop(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetDisconnect(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetFile(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetTick(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetStringCmd(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetSetConvar(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportNetSignonState(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcPrint(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcServerInfo(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcSendTable(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcClassInfo(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcSetPause(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcCreateStringTable(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcUpdateStringTable(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcVoiceInit(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcVoiceData(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcSounds(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcSetView(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcFixAngle(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcCrosshairAngle(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcBspDecal(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcUserMessage(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcEntityMessage(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcGameEvent(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcPacketEntities(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcTempEntities(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcPrefetch(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcMenu(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcGameEventList(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcGetCvarValue(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
		public static Task ExportSvcCmdKeyValues(ISourceWriterUtil bw, INetMessage message)
		{
			return Task.CompletedTask;
		}
	}
}