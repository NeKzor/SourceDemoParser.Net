using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcClassInfoMessage : INetMessage
	{
		public bool CreateOnClient { get; set; }
		public List<ServerClassInfo> ServerClasses { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt16();
			var create = buf.ReadBoolean();
			var servers = new List<ServerClassInfo>(length);
			if (!create)
			{
				while (length-- > 0)
				{
					servers.Add(new ServerClassInfo()
					{
						ClassId = (short)buf.ReadBits((int)System.Math.Log(length, 2) + 1),
						ClassName = buf.ReadString(),
						DataTableName = buf.ReadString()
					});
				}
			}
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}