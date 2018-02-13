using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcUpdateStringTableMessage : INetMessage
	{
		public int Id { get; set; }
		public bool EntriesChanged { get; set; }
		public int ChangedEntries { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var id = buf.ReadBits(5);
			var changed = buf.ReadBoolean();
			var entries = (changed) ? buf.ReadInt16() : 1;
			var length = buf.ReadBits(20);
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