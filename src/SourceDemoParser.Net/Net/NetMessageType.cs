namespace SourceDemoParser
{
	public abstract class NetMessageType
	{
		public int MessageType { get; set; }
		public string Name { get; set; }

		public NetMessageType(int code)
		{
			MessageType = code;
			Name = GetType().Name;
		}

		public abstract INetMessage GetMessage();

		public static NetMessageType Empty = default(NetMessageType);

		public override string ToString()
			=> Name;
	}
}