using System.Collections.Generic;

namespace SourceDemoParser
{
	public class NetSetConVarMessage : INetMessage
	{
		public List<ConVar> ConVars { get; set; }
	}
}