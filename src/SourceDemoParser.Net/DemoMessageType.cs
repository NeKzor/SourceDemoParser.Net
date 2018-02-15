namespace SourceDemoParser
{
	public abstract class DemoMessageType
	{
		public int MessageType { get; }
		public string Name { get; }

		public DemoMessageType(int code)
		{
			MessageType = code;
			Name = GetType().Name;
		}

		public abstract IDemoMessage GetMessage();

		public override string ToString()
			=> Name;
	}
	public abstract class DemoMessageType<T> : DemoMessageType
		where T : IDemoMessage, new()
	{
		public DemoMessageType(int code) : base(code)
		{
		}

		public override IDemoMessage GetMessage()
		{
			var message = new T();
			message.Type = this;
			return message;
		}
	}
}