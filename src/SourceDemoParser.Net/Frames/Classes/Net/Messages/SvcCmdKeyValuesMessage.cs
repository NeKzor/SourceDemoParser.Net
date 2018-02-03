namespace SourceDemoParser
{
	public class SvcCmdKeyValuesMessage : INetMessage
	{
		public uint Length { get; set; }
		public byte[] Data { get; set; }
	}
}