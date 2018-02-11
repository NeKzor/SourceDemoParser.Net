namespace SourceDemoParser
{
	public class SvcUserMessageMessage : INetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}