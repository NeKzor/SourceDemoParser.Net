using System;

namespace SourceDemoParser.Net
{
	public class MessageTypeException : Exception
	{
		public DemoMessageType Type { get; private set; }
		public int CurrentTick { get; private set; }
		
		
		public MessageTypeException(int tick, DemoMessageType type)
			: base($"[{tick}] Unknown demo message type: {(int)type}")
		{
			Type = type;
			CurrentTick = tick;
		}
	}
}