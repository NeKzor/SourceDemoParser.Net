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

		Task IFrame.ParseData(SourceDemo demo)
		{
			ConsoleCommand = Encoding.ASCII
				.GetString(RawData)
				.TrimEnd(new char[1]);
			return Task.CompletedTask;
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			$"{ConsoleCommand}\0".ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}