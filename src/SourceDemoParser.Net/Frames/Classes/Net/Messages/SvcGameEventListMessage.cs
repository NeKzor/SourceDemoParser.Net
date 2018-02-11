namespace SourceDemoParser
{
	public class SvcGameEventListMessage : INetMessage
	{
		public int Events { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}