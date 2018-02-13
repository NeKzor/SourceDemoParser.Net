using System;
using System.Text;

namespace SourceDemoParser
{
	internal static class InternalExtensions
	{
		public static byte[] ToBytes(this byte[] data)
		{
			var bytes = new byte[4 + data.Length];
			var length = data.Length.ToBytes();
			int i = 0;
			for (; i < 4; i++)
				bytes[i] = length[i];
			for (; i < bytes.Length; i++)
				bytes[i] = data[i - 4];
			return bytes;
		}
		public static byte[] ToBytes(this int data)
		{
			var bytes = BitConverter.GetBytes(data);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}
		public static byte[] ToBytes(this float data)
		{
			var bytes = BitConverter.GetBytes(data);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}
		public static byte[] ToBytes(this string data, bool withLength = true)
		{
			if (withLength)
			{
				var bytes = BitConverter.GetBytes(data.Length);
				if (!BitConverter.IsLittleEndian)
					Array.Reverse(bytes);
				Encoding.ASCII.GetBytes(data).AppendTo(ref bytes);
				return bytes;
			}
			return Encoding.ASCII.GetBytes(data);
		}
		public static void AppendTo(this byte[] source, ref byte[] destination)
		{
			var temp = destination;
			destination = new byte[temp.Length + source.Length];
			temp.CopyTo(destination, 0);
			source.CopyTo(destination, temp.Length);
		}
		public static byte[] Merge(params byte[][] sources)
		{
			var bytes = new byte[0];
			foreach (var source in sources)
				bytes.AppendTo(ref bytes);

			return bytes;
		}
		public static byte[] ToBuffer(this byte[] data, int length)
		{
			var buffer = new byte[length];
			for (int i = 0; i < data.Length; i++)
				buffer[i] = data[i];
			return buffer;
		}
	}
}