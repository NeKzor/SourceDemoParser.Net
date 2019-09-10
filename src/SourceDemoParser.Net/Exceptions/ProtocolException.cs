using System;

namespace SourceDemoParser
{
    public class ProtocolException : Exception
    {
        public int ProtocolVersion { get; }

        public ProtocolException(int protocol)
            : base($"Unsupported demo protocol version: {protocol}.")
        {
            ProtocolVersion = protocol;
        }

        // Generated
        public ProtocolException() : base()
        {
        }
        public ProtocolException(string message) : base(message)
        {
        }
        public ProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
