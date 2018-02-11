namespace SourceDemoParser
{
	public class SvcUpdateStringTableMessage : INetMessage
	{
		public int Id { get; set; }
		public bool EntriesChanged { get; set; }
		public int ChangedEntries { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
	}
}