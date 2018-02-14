using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetSetConVarMessage : NetMessage
	{
		public List<ConVar> ConVars { get; set; }

		public NetSetConVarMessage()
		{
			ConVars = new List<ConVar>();
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt32();
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
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteInt32(ConVars.Count);
			foreach (var convar in ConVars)
			{
				bw.WriteString(convar.Name);
				bw.WriteString(convar.Value);
			}
			return Task.CompletedTask;
		}
	}
}