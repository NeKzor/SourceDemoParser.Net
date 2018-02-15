using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceDemoParser
{
	// Thanks Traderain
	[DebuggerDisplay("{CurrentByte,nq}/{Length,nq}")]
	internal class BitBuffer : ISourceBufferUtil
	{
		internal enum EndianType
		{
			Little,
			Big
		}

		public byte[] Data => _data.ToArray();
		public int Length => _data.Count;
		public int CurrentBit => _currentBit;
		public int CurrentByte => (_currentBit - (_currentBit % 8)) / 8;
		public int BitsLeft => (_data.Count * 8) - _currentBit;
		public int BytesLeft => _data.Count - CurrentByte;

		internal EndianType Endian { get; set; }

		private readonly List<byte> _data;
		private int _currentBit;

		public BitBuffer(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Data cannot be null.");
			_data = new List<byte>(data);
		}

		public void SeekBits(int count, SeekOrigin origin = SeekOrigin.Current)
		{
			switch (origin)
			{
				case SeekOrigin.Current:
					_currentBit += count;
					break;
				case SeekOrigin.Begin:
					_currentBit = count;
					break;
				case SeekOrigin.End:
					_currentBit = (_data.Count * 8) - count;
					break;
			}

			if (_currentBit < 0 || _currentBit > _data.Count * 8)
				throw new InvalidOperationException();
		}
		public void SeekBytes(int count, SeekOrigin origin = SeekOrigin.Current)
		{
			SeekBits(count * 8, origin);
		}
		public void SkipBits()
		{
			var offset = _currentBit % 8;
			if (offset != 0) SeekBits(8 - offset);
		}

		public uint ReadUBits(int count)
		{
			return (Endian == EndianType.Little)
				? ReadUBitsLittleEndian(count)
				: ReadUBitsBigEndian(count);
		}
		public int ReadBits(int count)
		{
			var result = (int)ReadUBits(count - 1);
			if (ReadBoolean()) result = -((1 << (count - 1)) - result);
			return result;
		}
		public uint ReadOneUBit()
		{
			return ReadUBits(1);
		}
		public int ReadOneBit()
		{
			return ReadBits(2);
		}

		public bool ReadBoolean()
		{
			if (_currentBit + 1 > _data.Count * 8)
				throw new InvalidOperationException();

			var result = (_data[_currentBit / 8]
				& ((Endian == EndianType.Little)
					? 1 << _currentBit % 8
					: 128 >> _currentBit % 8)) != 0;
			_currentBit++;
			return result;
		}

		public byte ReadByte()
		{
			return (byte)ReadUBits(8);
		}
		public sbyte ReadSByte()
		{
			return (sbyte)ReadBits(8);
		}
		public byte[] ReadBytes(int count)
		{
			var result = new byte[count];
			for (var i = 0; i < count; i++)
				result[i] = ReadByte();
			return result;
		}
		public char ReadChar()
		{
			return (char)ReadByte();
		}
		public char[] ReadChars(int nChars)
		{
			var result = new char[nChars];
			for (var i = 0; i < nChars; i++)
				result[i] = (char)ReadByte();
			return result;
		}

		public short ReadInt16()
		{
			return (short)ReadBits(16);
		}
		public ushort ReadUInt16()
		{
			return (ushort)ReadUBits(16);
		}
		public int ReadInt32()
		{
			return ReadBits(32);
		}
		public uint ReadUInt32()
		{
			return ReadUBits(32);
		}

		public float ReadSingle()
		{
			// Am I doing this right?
			var temp = Endian;
			if ((!BitConverter.IsLittleEndian) && (temp == EndianType.Little))
				Endian = EndianType.Big;
			else if ((BitConverter.IsLittleEndian) && (temp == EndianType.Big))
				Endian = EndianType.Little;
			var result = BitConverter.ToSingle(ReadBytes(4).ToArray(), 0);
			Endian = temp;
			return result;
		}

		public string ReadString(int length, bool encodeToAscii = true)
		{
			var str = new List<byte>();
			while ((_data.Count - ((_currentBit - (_currentBit % 8)) / 8) != 0) && (length > 0))
			{
				var val = ReadByte();
				str.Add(val);
				length--;
			}
			return (encodeToAscii)
				? Encoding.ASCII.GetString(str.ToArray())
				: Encoding.UTF8.GetString(str.ToArray());
		}
		public string ReadString(bool encodeToAscii = true)
		{
			var str = new List<byte>();
			while (_data.Count - ((_currentBit - (_currentBit % 8)) / 8) != 0)
			{
				var val = ReadByte();
				if (val == 0x00) break;
				str.Add(val);
			}
			return (encodeToAscii)
				? Encoding.ASCII.GetString(str.ToArray())
				: Encoding.UTF8.GetString(str.ToArray());
		}

		private uint ReadUBitsBigEndian(int count)
		{
			if (count <= 0 || count > 32)
				throw new ArgumentException("Value must be a positive integer between 1 and 32 inclusive.", nameof(count));
			if (_currentBit + count > _data.Count * 8)
				throw new InvalidOperationException();

			var currentByte = _currentBit / 8;
			var bitOffset = _currentBit - (currentByte * 8);
			var nBytesToRead = (bitOffset + count) / 8;

			if ((bitOffset + count) % 8 != 0)
				nBytesToRead++;

			var currentValue = 0ul;
			for (var i = 0; i < nBytesToRead; i++)
				currentValue += (ulong)_data[currentByte + (nBytesToRead - 1) - i] << (i * 8);

			currentValue >>= (((nBytesToRead * 8) - bitOffset) - count);
			currentValue &= (uint)(((long)1 << count) - 1);

			_currentBit += count;
			return (uint)currentValue;
		}
		private uint ReadUBitsLittleEndian(int count)
		{
			count = Math.Abs(count);
			if (count <= 0 || count > 32)
				throw new ArgumentException("Value must be a positive integer between 1 and 32 inclusive.", nameof(count));

			if (_currentBit + count > _data.Count * 8)
				throw new InvalidOperationException();

			var currentByte = _currentBit / 8;
			var bitOffset = _currentBit - (currentByte * 8);
			var nBytesToRead = (bitOffset + count) / 8;

			if ((bitOffset + count) % 8 != 0)
				nBytesToRead++;

			var currentValue = 0ul;
			for (var i = 0; i < nBytesToRead; i++)
				currentValue += (ulong)_data[currentByte + i] << (i * 8);

			currentValue >>= bitOffset;
			currentValue &= (uint)(((long)1 << count) - 1);

			_currentBit += count;
			return (uint)currentValue;
		}
	}
}