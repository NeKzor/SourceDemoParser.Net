using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGetCvarValueMessage : NetMessage
	{
		public int Cookie { get; set; }
		public string CvarName { get; set; }

		public SvcGetCvarValueMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Cookie = buf.ReadInt32();
			CvarName = buf.ReadString();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteInt32(Cookie);
			bw.WriteString(CvarName);
			return Task.CompletedTask;
		}
	}
}