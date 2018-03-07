using System.Collections.Generic;

namespace SourceDemoParser
{
	public class SourceGame
	{
		public static readonly SourceGame Default = new SourceGame();

		public int? MaxSplitscreenClients { get; private set; }
		public bool HasAlignmentByte { get; private set; }
		public List<DemoMessageType> DefaultMessages { get; private set; }
		public List<NetMessageType> DefaultNetMessages { get; private set; }

		public SourceGame()
		{
			HasAlignmentByte = true;
			DefaultMessages = DemoMessages.Default;
			DefaultNetMessages = NetMessages.Default;
		}

		public SourceGame WithMaxSplitscreenClients(int maxSplitscreenClients)
		{
			MaxSplitscreenClients = maxSplitscreenClients;
			return this;
		}
		public SourceGame WithAlignmentByte(bool hasAlignmentByte)
		{
			HasAlignmentByte = hasAlignmentByte;
			return this;
		}
		public SourceGame WithDefaultDemoMessages(List<DemoMessageType> defaultMessages)
		{
			DefaultMessages = defaultMessages;
			return this;
		}
		public SourceGame WithDefaultNetMessages(List<NetMessageType> defaultNetMessages)
		{
			DefaultNetMessages = defaultNetMessages;
			return this;
		}
	}
}