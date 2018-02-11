namespace SourceDemoParser
{
	public class SvcSoundsMessage : INetMessage
	{
		public bool ReliableSound { get; set; }
		public int Length { get; set; }
		public int Sounds { get; set; }
		public byte[] Data { get; set; }
	}
}