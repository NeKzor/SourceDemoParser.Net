using System.Collections.Generic;

namespace SourceDemoParser
{
	public class SourceGame
	{
		public int? MaxSplitscreenClients { get; set; }
		public bool HasAlignmentByte { get; set; }

		public List<DemoMessageType> DefaultMessages { get; set; }
		public List<NetMessageType> DefaultNetMessages { get; set; }

		public SourceGame()
		{
			MaxSplitscreenClients = null;
			HasAlignmentByte = true;
			DefaultMessages = DemoMessages.Default;
			DefaultNetMessages = NetMessages.Default;
		}
	}
}