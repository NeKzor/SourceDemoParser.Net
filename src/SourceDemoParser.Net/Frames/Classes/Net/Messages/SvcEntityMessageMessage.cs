namespace SourceDemoParser
{
	public class SvcEntityMessageMessage : INetMessage
	{
		public uint EntityIndex { get; set; }
		public uint ClassId { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}