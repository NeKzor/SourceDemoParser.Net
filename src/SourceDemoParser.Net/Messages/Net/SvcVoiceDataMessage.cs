using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcVoiceDataMessage : INetMessage
	{
		public byte FromClient { get; set; }
		public byte Proximity { get; set; }
		public ushort Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var client = buf.ReadByte();
			var proximity = buf.ReadByte();
			var length = buf.ReadUInt16();
			var data = buf.ReadBytes(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}