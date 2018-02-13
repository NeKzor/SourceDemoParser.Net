using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcUserMessageMessage : INetMessage
	{
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var type = buf.ReadBits(8);
			var length = buf.ReadBits(11);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}