namespace SourceDemoParser
{
	public class NetSignonStateMessage : INetMessage
	{
		public byte SignonState { get; set; }
		public int SpawnCount { get; set; }
	}
}