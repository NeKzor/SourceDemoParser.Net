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
		public GameSpecific Game { get; private set; }

		public SourceDemo()
			=> Game = new GameSpecific();
	}

	public class GameSpecific
	{
		public int MaxSplitscreenClients { get; set; }
		public bool HasAlignmentByte { get; set; }
		public List<DemoMessageType> DefaultMessages { get; set; }

		public GameSpecific()
		{
			MaxSplitscreenClients = 1;
			HasAlignmentByte = true;
			DefaultMessages = DemoMessages.Default;
		}
	}
}