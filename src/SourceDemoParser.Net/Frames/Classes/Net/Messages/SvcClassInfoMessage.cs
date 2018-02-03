using System.Collections.Generic;

namespace SourceDemoParser
{
	public class SvcClassInfoMessage : INetMessage
	{
		public bool CreateOnClient { get; set; }
		public List<ServerClassInfo> ServerClasses { get; set; }

	}
}