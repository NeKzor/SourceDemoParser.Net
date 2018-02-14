using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcEntityMessageMessage : NetMessage
	{
		public int EntityIndex { get; set; }
		public int ClassId { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
		
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			EntityIndex = buf.ReadBits(11); // MAX_EDICT_BITS
			ClassId = buf.ReadBits(9); // MAX_SERVER_CLASS_BITS
			Length = buf.ReadBits(11);
			System.Diagnostics.Debug.WriteLine(EntityIndex);
			System.Diagnostics.Debug.WriteLine(ClassId);
			System.Diagnostics.Debug.WriteLine(Length);
			buf.SeekBits(Length);
			//var data = buf.ReadBytes((int)Length / 8);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBits(EntityIndex, 11);
			bw.WriteBits(ClassId, 9);
			bw.WriteBits(Length, 11);
			//bw.WriteBits(0, Length);
			return Task.CompletedTask;
		}
	}
}