namespace SourceDemoParser
{
	public class SvcServerInfoMessage : INetMessage
	{
		public short Protocol { get; set; }
		public int ServerCount { get; set; }
		public bool IsHltv { get; set; }
		public bool IsDedicated { get; set; }
		public int ClientCrc { get; set; }
		public ushort MaxClasses { get; set; }
		public int MapCrc { get; set; }
		public byte PlayerSlot { get; set; }
		public byte MaxClients { get; set; }
		public float TickInterval { get; set; }
		public char OperatingSystem { get; set; }
		public string GameDir { get; set; }
		public string MapName { get; set; }
		public string SkyName { get; set; }
		public string HostName { get; set; }
	}
}