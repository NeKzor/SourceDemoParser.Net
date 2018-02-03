namespace SourceDemoParser
{
	public class SvcPacketEntitiesMessage : INetMessage
	{
		public uint MaxEntries { get; set; }
		public bool IsDelta { get; set; }
		public int DeltaFrom { get; set; }
		public uint BaseLine { get; set; }
		public uint UpdatedEntries { get; set; }
		public uint Length { get; set; }
		public bool UpdateBaseline { get; set; }
		public byte[] Data { get; set; }
	}
}