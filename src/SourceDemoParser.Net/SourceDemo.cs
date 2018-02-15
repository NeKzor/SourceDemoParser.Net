using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		// Configuration
		public SourceGame Game { get; private set; }

		public SourceDemo()
			=> Game = new SourceGame();
	}
}