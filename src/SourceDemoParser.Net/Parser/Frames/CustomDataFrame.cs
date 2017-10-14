using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class CustomDataFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public int Unknown1 { get; set; }
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

		Task IFrame.ParseData()
		{
			var buf = new BitBuffer(RawData);
			Unknown2 = buf.ReadInt32();
			Unknown3 = buf.ReadString();
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = Unknown1.GetBytes();
			var data = Unknown2.GetBytes();
			$"{Unknown3}\0".GetBytes().AppendTo(ref data);
			data.Length.GetBytes().AppendTo(ref bytes);
			data.AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> "TODO";
	}
}