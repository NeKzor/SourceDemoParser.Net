namespace SourceDemoParser
{
	public abstract class NetMessageType
	{
		public static readonly NetMessageType Empty = default(NetMessageType);

		public int MessageType { get; }
		public string Name { get; }

		public NetMessageType(int code)
		{
			MessageType = code;
			Name = GetType().Name;
		}

		public abstract INetMessage GetMessage();

		public override string ToString()
			=> Name;
	}

	public abstract class NetMessageType<T> : NetMessageType
		where T : INetMessage, new()
	{
		public NetMessageType(int code) : base(code)
		{
		}

		public override INetMessage GetMessage()
		{
			var message = new T();
			message.Type = this;
			return message;
		}

		public override string ToString()
			=> Name;
	}
}