using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcPacketEntitiesMessage : INetMessage
	{
		public int MaxEntries { get; set; }
		public bool IsDelta { get; set; }
		public int DeltaFrom { get; set; }
		public bool BaseLine { get; set; }
		public int UpdatedEntries { get; set; }
		public int Length { get; set; }
		public bool UpdateBaseline { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var max = buf.ReadBits(11); // ?
			var delta = buf.ReadBoolean();
			var from = (delta) ? buf.ReadInt32() : 0;
			//var length = buf.ReadUInt32();
			var baseline = buf.ReadBoolean();
			var entries = buf.ReadBits(11); // ?
			var length = buf.ReadBits(20);
			var update = buf.ReadBoolean();
			buf.SeekBits(length);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}