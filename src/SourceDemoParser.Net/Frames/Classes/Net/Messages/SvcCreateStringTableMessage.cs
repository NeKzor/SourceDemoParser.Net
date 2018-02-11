namespace SourceDemoParser
{
	public class SvcCreateStringTableMessage : INetMessage
	{
		public string TableName { get; set; }
		public short MaxEntries { get; set; }
		public int Entries { get; set; }
		public int Length { get; set; }
		public bool UserDataFixedSize { get; set; }
		public int UserDataSize { get; set; }
		public int UserDataSizeBits { get; set; }
		public byte[] Data { get; set; }
	}
}