using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class ConsoleCmdFrame : IDemoFrame
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

		Task IDemoFrame.Parse(SourceDemo demo)
		{
			ConsoleCommand = Encoding.ASCII
				.GetString(RawData)
				.TrimEnd(new char[1]);
			return Task.CompletedTask;
		}
		Task<byte[]> IDemoFrame.Export()
		{
			var data = new byte[0];
			$"{ConsoleCommand}\0".ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}