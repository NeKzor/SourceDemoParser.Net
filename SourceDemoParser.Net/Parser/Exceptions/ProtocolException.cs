using System;

namespace SourceDemoParser.Net
{
	public class ProtocolException : Exception
	{
		public int ProtocolVersion { get; private set; }
		
		public ProtocolException(int protocol)
			: base($"Unsupported demo protocol version: {protocol}")
		{
			ProtocolVersion = protocol;
		}
	}
}