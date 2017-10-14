using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class ConsoleCmdFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public string ConsoleCommand { get; set; }

		public ConsoleCmdFrame()
		{
		}
		public ConsoleCmdFrame(byte[] data)
		{
			RawData = data;
		}

		Task IFrame.ParseData()
		{
			ConsoleCommand = Encoding.ASCII.GetString(RawData).TrimEnd(new char[1]);
			return Task.FromResult(true);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = $"{ConsoleCommand}\0".GetBytes();
			//var bytes = RawData.GetBytes();
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> (!string.IsNullOrEmpty(ConsoleCommand)) ? $"{ConsoleCommand}" : "NULL";
	}
}