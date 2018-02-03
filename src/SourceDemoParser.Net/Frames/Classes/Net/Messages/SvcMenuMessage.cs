namespace SourceDemoParser
{
	public class SvcMenuMessage : INetMessage
	{
		public short Type { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}