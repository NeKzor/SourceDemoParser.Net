using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcMenuMessage : INetMessage
	{
		public short Type { get; set; }
		public uint Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var type = buf.ReadInt16();
			var length = buf.ReadUInt32();
			var data = buf.ReadBytes((int)length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}