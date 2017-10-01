using System;

namespace SourceDemoParser
{
	public class MessageTypeException : Exception
	{
		public DemoMessageType Type { get; }
		public int CurrentTick { get; }

		public MessageTypeException(int tick, DemoMessageType type)
			: base($"[{tick}] Unknown demo message type: {(int)type}.")
		{
			Type = type;
			CurrentTick = tick;
		}

		// Generated
		public MessageTypeException()
			: base()
		{
		}
		public MessageTypeException(string message)
			: base(message)
		{
		}
		public MessageTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		protected MessageTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}