namespace SourceDemoParser
{
	public class SvcUserMessageMessage : INetMessage
	{
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}