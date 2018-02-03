namespace SourceDemoParser
{
	public class NetFileMessage : INetMessage
	{
		public uint TransferId { get; set; }
		public string FileName { get; set; }
		public bool FileRequested { get; set; }
	}
}