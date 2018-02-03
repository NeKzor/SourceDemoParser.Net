namespace SourceDemoParser
{
	public class NetMessageData
	{
		public NetMessageType Type { get; set; }
		public INetMessage Message { get; set; }

		public override string ToString()
			=> Type.Name;
	}
}