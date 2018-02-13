using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcSendTableMessage : INetMessage
	{
		public bool NeedsDecoder { get; set; }
		public short Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var decoder = buf.ReadBoolean();
			var length = buf.ReadInt16();
			buf.SeekBits(length);
			//var data = buf.ReadBytes(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}