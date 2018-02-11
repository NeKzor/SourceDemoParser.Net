namespace SourceDemoParser
{
	public class SvcTempEntitiesMessage : INetMessage
	{
		public int Entries { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}