using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser.Net
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
			
			var bytes = BitConverter.GetBytes(RawData.Length);
			if (!BitConverter.IsLittleEndian)
    				Array.Reverse(bytes);
			bytes.Concat(RawData);
			return Task.FromResult(bytes);
		}
		
		public override string ToString()
			=> (!string.IsNullOrEmpty(ConsoleCommand)) ? $"{ConsoleCommand}" : "NULL";
	}
}