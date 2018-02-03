namespace SourceDemoParser
{
	public class SvcVoiceDataMessage : INetMessage
	{
		public byte FromClient { get; set; }
		public byte Proximity { get; set; }
		public ushort Length { get; set; }
		public byte[] Data { get; set; }
	}
}