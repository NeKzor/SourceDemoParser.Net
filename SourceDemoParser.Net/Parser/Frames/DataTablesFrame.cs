using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser.Net
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
			
			var bytes = BitConverter.GetBytes(RawData.Length);
			if (!BitConverter.IsLittleEndian)
    				Array.Reverse(bytes);
			bytes.Concat(RawData);
			return Task.FromResult(bytes);
		}
		
		public override string ToString()
			=> "TODO";
	}
}