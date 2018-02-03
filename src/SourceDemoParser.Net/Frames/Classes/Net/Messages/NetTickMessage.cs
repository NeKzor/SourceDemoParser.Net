namespace SourceDemoParser
{
	public class NetTickMessage : INetMessage
	{
		public int Tick { get; set; }
		public uint HostFrameTime { get; set; }
		public uint HostFrameTimeStdDeviation { get; set; }
	}
}