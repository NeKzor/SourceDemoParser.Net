using System;

namespace SourceDemoParser.Net
{
	public class SourceException : Exception
	{
		public string FileStamp { get; private set; }
		
		public SourceException(string fileStamp)
			: base($"Unknown file stamp: {fileStamp} (expected: HL2DEMO\0)")
		{
			FileStamp = fileStamp;
		}
	}
}