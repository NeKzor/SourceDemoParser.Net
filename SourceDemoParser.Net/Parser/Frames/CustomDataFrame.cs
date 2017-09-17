using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser.Net
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
			
			var bytes = BitConverter.GetBytes(Unknown);
			if (!BitConverter.IsLittleEndian)
    				Array.Reverse(bytes);
			
			var data = BitConverter.GetBytes(RawData.Length);
			if (!BitConverter.IsLittleEndian)
    				Array.Reverse(data);
			
			bytes.Concat(data);
			bytes.Concat(RawData);
			return Task.FromResult(bytes);
		}
		
		public override string ToString()
			=> "TODO";
	}
}