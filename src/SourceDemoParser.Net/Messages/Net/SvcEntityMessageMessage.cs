using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcEntityMessageMessage : INetMessage
	{
		public int EntityIndex { get; set; }
		public int ClassId { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var index = buf.ReadBits(11); // MAX_EDICT_BITS
			var id = buf.ReadBits(9); // MAX_SERVER_CLASS_BITS
			var length = buf.ReadBits(11);
			Debug.WriteLine(index);
			Debug.WriteLine(id);
			Debug.WriteLine(length);
			buf.SeekBits(length);
			//var data = buf.ReadBytes((int)length / 8);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}