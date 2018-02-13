using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcClassInfoMessage : NetMessage
	{
		public bool CreateOnClient { get; set; }
		public List<ServerClassInfo> ServerClasses { get; set; }

		public SvcClassInfoMessage(NetMessageType type) : base(type)
		{
			ServerClasses = new List<ServerClassInfo>();
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var length = buf.ReadInt16();
			CreateOnClient = buf.ReadBoolean();
			if (!CreateOnClient)
			{
				while (length-- > 0)
				{
					ServerClasses.Add(new ServerClassInfo()
					{
						ClassId = (short)buf.ReadBits((int)System.Math.Log(length, 2) + 1),
						ClassName = buf.ReadString(),
						DataTableName = buf.ReadString()
					});
				}
			}
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			var length = (short)ServerClasses.Count;
			bw.WriteInt16(length);
			bw.WriteBoolean(CreateOnClient);
			if (!CreateOnClient)
			{
				foreach (var sclass in ServerClasses)
				{
					length--;
					bw.WriteBits(sclass.ClassId, (int)System.Math.Log(length, 2) + 1);
					bw.WriteString(sclass.ClassName);
					bw.WriteString(sclass.DataTableName);
				}
			}
			return Task.CompletedTask;
		}
	}
}