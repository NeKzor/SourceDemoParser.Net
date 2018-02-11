namespace SourceDemoParser
{
	public class SvcGameEventMessage : INetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}