using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCmdKeyValuesMessage : INetMessage
	{
		public uint Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
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