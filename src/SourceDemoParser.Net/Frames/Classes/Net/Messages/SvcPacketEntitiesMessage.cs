namespace SourceDemoParser
{
	public class SvcPacketEntitiesMessage : INetMessage
	{
		public int MaxEntries { get; set; }
		public bool IsDelta { get; set; }
		public int DeltaFrom { get; set; }
		public bool BaseLine { get; set; }
		public int UpdatedEntries { get; set; }
		public int Length { get; set; }
		public bool UpdateBaseline { get; set; }
		public byte[] Data { get; set; }
	}
}