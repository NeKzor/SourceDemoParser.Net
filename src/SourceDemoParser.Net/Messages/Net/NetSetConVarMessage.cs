using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetSetConVarMessage : INetMessage
	{
		public List<ConVar> ConVars { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt32();
			ConVars = new List<ConVar>(length);
			while (length-- != 0)
			{
				ConVars.Add(new ConVar()
				{
					Name = buf.ReadString(),
					Value = buf.ReadString()
				});
			}
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}