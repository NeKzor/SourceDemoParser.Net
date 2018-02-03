namespace SourceDemoParser
{
	public class SvcGameEventMessage : INetMessage
	{
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}