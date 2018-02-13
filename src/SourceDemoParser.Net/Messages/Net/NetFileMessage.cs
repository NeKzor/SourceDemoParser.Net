using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetFileMessage : INetMessage
	{
		public uint TransferId { get; set; }
		public string FileName { get; set; }
		public bool FileRequested { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			TransferId = buf.ReadUInt32();
			FileName = buf.ReadString();
			FileRequested = buf.ReadBoolean();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteUInt32(TransferId);
			bw.WriteString(FileName);
			bw.WriteBoolean(FileRequested);
			return Task.CompletedTask;
		}
	}
}