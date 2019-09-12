using System;
using System.Text;

namespace SourceDemoParser
{
    internal static class InternalExtensions
    {
        public static byte[] ToBytes(this ReadOnlySpan<char> str, bool withLength = true)
        {
            var bytes = default(byte[]);
            if (withLength)
            {
                bytes = new byte[str.Length + 4];
                BitConverter.GetBytes(str.Length).CopyTo(bytes, 0);
                Encoding.ASCII.GetBytes(str.ToArray()).CopyTo(bytes.AsSpan(4));
            }
            else
            {
                bytes = new byte[str.Length];
                Encoding.ASCII.GetBytes(str.ToArray()).CopyTo(bytes.AsSpan());
            }
            return bytes;
        }
        public static byte[] ToBuffer(this Span<byte> data, int length)
        {
            var buffer = new byte[length];
            data.CopyTo(buffer);
            return buffer;
        }
    }
}
