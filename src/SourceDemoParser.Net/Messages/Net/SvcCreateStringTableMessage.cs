using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCreateStringTableMessage : NetMessage
	{
		public string TableName { get; set; }
		public short MaxEntries { get; set; }
		public int Entries { get; set; }
		public int Length { get; set; }
		public bool UserDataFixedSize { get; set; }
		public int UserDataSize { get; set; }
		public int UserDataSizeBits { get; set; }
		public bool Compressed { get; set; }
		public byte[] Data { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			TableName = buf.ReadString(); // 256
			System.Diagnostics.Debug.WriteLine(TableName);

			MaxEntries = buf.ReadInt16();
			System.Diagnostics.Debug.WriteLine(MaxEntries);

			var toread = (int)System.Math.Log(MaxEntries, 2) + 1;
			Entries = buf.ReadBits((toread == 1) ? 2 : toread);
			System.Diagnostics.Debug.WriteLine(Entries);

			Length = buf.ReadBits(20); // NET_MAX_PALYLOAD_BITS + 3
			System.Diagnostics.Debug.WriteLine(Length);

			UserDataFixedSize = buf.ReadBoolean();
			System.Diagnostics.Debug.WriteLine(UserDataFixedSize);

			UserDataSize = (UserDataFixedSize) ? buf.ReadBits(12) : 0;
			UserDataSizeBits = (UserDataFixedSize) ? buf.ReadBits(4) : 0;
			Compressed = buf.ReadBoolean();
			System.Diagnostics.Debug.WriteLine(Compressed);

			buf.SeekBits(Length);
			//var data = buf.ReadBytes(Length / 8);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(TableName);
			bw.WriteInt16(MaxEntries);
			var towrite = (int)System.Math.Log(MaxEntries, 2) + 1;
			bw.WriteBits(Entries, (towrite == 1) ? 2 : towrite);
			bw.WriteBits(Length, 20);
			bw.WriteBoolean(UserDataFixedSize);
			if (UserDataFixedSize) bw.WriteBits(UserDataSize, 12);
			if (UserDataFixedSize) bw.WriteBits(UserDataSizeBits, 4);
			bw.WriteBoolean(Compressed);
			//bw.WriteBits(0, Length);
			return Task.CompletedTask;
		}
	}
}