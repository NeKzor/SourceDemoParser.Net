using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcGetCvarValueMessage : INetMessage
	{
		public int Cookie { get; set; }
		public string CvarName { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var cookie = buf.ReadInt32();
			var name = buf.ReadString();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}