namespace SourceDemoParser
{
	public class NetTickMessage : INetMessage
	{
		public int Tick { get; set; }
		public short HostFrameTime { get; set; }
		public short HostFrameTimeStdDeviation { get; set; }
	}
}