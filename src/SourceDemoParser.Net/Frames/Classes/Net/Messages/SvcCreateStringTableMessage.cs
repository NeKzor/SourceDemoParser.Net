namespace SourceDemoParser
{
	public class SvcCreateStringTableMessage : INetMessage
	{
		public string TableName { get; set; }
		public uint Entries { get; set; }
		public uint Length { get; set; }
		public bool UserDataFixedSize { get; set; }
		public uint UserDataSize { get; set; }
		public uint UserDataSizeBits { get; set; }
		public byte[] Data { get; set; }
	}
}