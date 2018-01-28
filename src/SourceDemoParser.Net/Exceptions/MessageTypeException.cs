using System;

namespace SourceDemoParser
{
	public class MessageTypeException : Exception
	{
		public int Type { get; }
		public long Position { get; set; }

		public MessageTypeException(int type, long position)
			: base($"Unknown demo message type 0x{type.ToString("X")} at {position}.")
		{
			Type = type;
			Position = position;
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