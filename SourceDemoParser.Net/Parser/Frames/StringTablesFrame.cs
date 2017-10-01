using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class StringTablesFrame : IFrame
	{
		public byte[] RawData { get; set; }

		public StringTablesFrame()
		{
		}
		public StringTablesFrame(byte[] data)
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
			=> "NULL";
	}
}