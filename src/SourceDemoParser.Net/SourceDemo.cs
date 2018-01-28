using System.Collections.Generic;

namespace SourceDemoParser
{
	public class SourceDemo
	{
		// Header
		public string HeaderId { get; set; }
		public int Protocol { get; set; }
		public int NetworkProtocol { get; set; }
		public string ServerName { get; set; }
		public string ClientName { get; set; }
		public string MapName { get; set; }
		public string GameDirectory { get; set; }
		public float PlaybackTime { get; set; }
		public int PlaybackTicks { get; set; }
		public int PlaybackFrames { get; set; }
		public int SignOnLength { get; set; }

		// Data
		public List<IDemoMessage> Messages { get; set; }

		// For the parser
		internal int MaxSplitscreenClients { get; set; }
		internal bool HasAlignmentByte { get; set; }
		internal List<DemoMessageType> GameMessages { get; set; }

		public SourceDemo()
		{
			MaxSplitscreenClients = 1;
			HasAlignmentByte = true;
			GameMessages = DemoMessages.Default;
		}
	}
}