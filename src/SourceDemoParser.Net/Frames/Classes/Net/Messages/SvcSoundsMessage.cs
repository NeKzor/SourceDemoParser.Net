namespace SourceDemoParser
{
	public class SvcSoundsMessage : INetMessage
	{
		public bool ReliableSound { get; set; }
		public uint Length { get; set; }
		public uint Sounds { get; set; }
		public byte[] Data { get; set; }
	}
}