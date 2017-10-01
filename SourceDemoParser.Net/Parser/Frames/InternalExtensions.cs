using System;
using System.Text;

namespace SourceDemoParser
{
	internal static class InternalExtensions
	{
		internal static byte[] GetBytes(this byte[] data)
		{
			var bytes = new byte[4 + data.Length];
			var length = data.Length.GetBytes();
			int i = 0;
			for (; i < 4; i++)
				bytes[i] = length[i];
			for (; i < bytes.Length; i++)
				bytes[i] = data[i - 4];
			return bytes;
		}

		internal static byte[] GetBytes(this int data)
		{
			var bytes = BitConverter.GetBytes(data);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}

		internal static byte[] GetBytes(this float data)
		{
			var bytes = BitConverter.GetBytes(data);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}

		internal static byte[] GetBytes(this string data)
		{
			var bytes = BitConverter.GetBytes(data.Length);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			Encoding.ASCII.GetBytes(data).AppendTo(ref bytes);
			return bytes;
		}

		internal static void AppendTo(this byte[] source, ref byte[] destination)
		{
			var temp = destination;
			destination = new byte[temp.Length + source.Length];
			temp.CopyTo(destination, 0);
			source.CopyTo(destination, temp.Length);
		}

		internal static byte[] Merge(params byte[][] sources)
		{
			var bytes = new byte[0];
			foreach (var source in sources)
				bytes.AppendTo(ref bytes);

			return bytes;
		}
	}
}