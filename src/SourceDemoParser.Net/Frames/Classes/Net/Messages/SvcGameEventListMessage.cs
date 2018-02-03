namespace SourceDemoParser
{
	public class SvcGameEventListMessage : INetMessage
	{
		public uint Events { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}