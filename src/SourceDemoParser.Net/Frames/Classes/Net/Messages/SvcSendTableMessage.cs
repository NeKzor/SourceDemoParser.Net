namespace SourceDemoParser
{
	public class SvcSendTableMessage : INetMessage
	{
		public bool NeedsDecoder { get; set; }
		public short Length { get; set; }
		public byte[] Data { get; set; }
	}
}