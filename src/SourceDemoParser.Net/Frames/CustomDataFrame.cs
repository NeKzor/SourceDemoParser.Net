using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class CustomDataFrame : IFrame
	{
		public int Unknown1 { get; set; }
		public byte[] RawData { get; set; }

		public int Unknown2 { get; set; }
		public string Unknown3 { get; set; }

		public CustomDataFrame()
		{
		}
		public CustomDataFrame(int idk, byte[] data)
		{
			Unknown1 = idk;
			RawData = data;
		}

		Task IFrame.ParseData(SourceDemo demo)
		{
			var buf = new BitBuffer(RawData);
			Unknown2 = buf.ReadInt32();
			Unknown3 = buf.ReadString();
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			Unknown1.ToBytes().AppendTo(ref data);
			Unknown2.ToBytes().AppendTo(ref data);
			$"{Unknown3}\0".ToBytes(false).AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}