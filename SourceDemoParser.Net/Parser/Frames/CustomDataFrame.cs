using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class CustomDataFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public int Unknown { get; set; }

		public CustomDataFrame()
		{
		}
		public CustomDataFrame(int idk, byte[] data)
		{
			Unknown = idk;
			RawData = data;
		}

		Task IFrame.ParseData()
		{
			// Todo
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = Unknown.GetBytes();
			RawData.GetBytes().AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> "NULL";
	}
}