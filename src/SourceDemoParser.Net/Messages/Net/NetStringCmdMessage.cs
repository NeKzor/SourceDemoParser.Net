using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetStringCmdMessage : INetMessage
	{
		public string Command { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Command = buf.ReadString(256); // 256 MAX_COMMAND_LEN
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(Command);
			return Task.CompletedTask;
		}
	}
}