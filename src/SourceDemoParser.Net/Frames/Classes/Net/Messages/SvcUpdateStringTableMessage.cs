namespace SourceDemoParser
{
	public class SvcUpdateStringTableMessage : INetMessage
	{
		public uint Id { get; set; }
		public bool EntriesChanged { get; set; }
		public ushort ChangedEntries { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}