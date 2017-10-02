using System;

namespace SourceDemoParser
{
	public class SourceException : Exception
	{
		public string FileStamp { get; }

		public SourceException(string fileStamp)
			: base($"Unknown file stamp: {fileStamp} (expected: HL2DEMO\0).")
		{
			FileStamp = fileStamp;
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