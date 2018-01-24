using System;

namespace SourceDemoParser
{
	public class MessageTypeException : Exception
	{
		public DemoMessageType Type { get; }
		public int CurrentTick { get; }

		public MessageTypeException(IDemoMessage message)
			: base($"[{message.CurrentTick}] Unknown demo message type: {(int)message.Type}.")
		{
			Type = message.Type;
			CurrentTick = message.CurrentTick;
		}

		// Generated
		public MessageTypeException() : base()
		{
		}
		public MessageTypeException(string message) : base(message)
		{
		}
		public MessageTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}