namespace SourceDemoParser
{
	public class SvcEntityMessageMessage : INetMessage
	{
		public int EntityIndex { get; set; }
		public int ClassId { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}