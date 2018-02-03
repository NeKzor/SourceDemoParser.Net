namespace SourceDemoParser
{
	public class SvcTempEntitiesMessage : INetMessage
	{
		public uint Entries { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}