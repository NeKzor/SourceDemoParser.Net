using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class DataTablesFrame : IFrame
	{
		public byte[] RawData { get; set; }

		public DataTablesFrame()
		{
		}
		public DataTablesFrame(byte[] data)
		{
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

			var bytes = RawData.GetBytes();
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> "TODO";
	}
}