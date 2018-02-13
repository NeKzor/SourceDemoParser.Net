using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCreateStringTableMessage : INetMessage
	{
		public string TableName { get; set; }
		public short MaxEntries { get; set; }
		public int Entries { get; set; }
		public int Length { get; set; }
		public bool UserDataFixedSize { get; set; }
		public int UserDataSize { get; set; }
		public int UserDataSizeBits { get; set; }
		public byte[] Data { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var name = buf.ReadString(); // 256
			System.Diagnostics.Debug.WriteLine(name);

			var max = buf.ReadInt16();
			System.Diagnostics.Debug.WriteLine(max);

			var toread = (int)System.Math.Log(max, 2) + 1;
			var entries = buf.ReadBits((toread == 1) ? 2 : toread);
			System.Diagnostics.Debug.WriteLine(entries);

			var length = buf.ReadBits(20); // NET_MAX_PALYLOAD_BITS + 3
			System.Diagnostics.Debug.WriteLine(length);

			var fixedsize = buf.ReadBoolean();
			System.Diagnostics.Debug.WriteLine(fixedsize);

			var size = (fixedsize) ? buf.ReadBits(12) : 0;
			var bits = (fixedsize) ? buf.ReadBits(4) : 0;
			var compressed = buf.ReadBoolean();
			System.Diagnostics.Debug.WriteLine(compressed);

			buf.SeekBits(length);
			//var data = buf.ReadBytes(length / 8);
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}