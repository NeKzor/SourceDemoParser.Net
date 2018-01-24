using System;

namespace SourceDemoParser
{
	public class SourceException : Exception
	{
		public string HeaderId { get; }

		public SourceException(string headerId)
			: base($"Unknown DEMO_HEADER_ID: {headerId} (expected: HL2DEMO\0).")
		{
			HeaderId = headerId;
		}

		// Generated
		public SourceException() : base()
		{
		}
		public SourceException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}