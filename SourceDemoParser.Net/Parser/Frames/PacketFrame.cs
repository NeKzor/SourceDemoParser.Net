using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Net.Extensions;

namespace SourceDemoParser.Net
{
	public class PacketFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public Vector3f PlayerPosition { get; set; }
		
		public PacketFrame()
		{
		}
		public PacketFrame(byte[] data)
		{
			RawData = data;
		}
		public PacketFrame(Vector3f position)
		{
			PlayerPosition = position;
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
			=> PlayerPosition.ToString();
	}
}