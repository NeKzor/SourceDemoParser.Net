using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SourceDemoParser
{
	// Thanks Traderain
	internal enum EndianType
	{
		Little,
		Big
	}

	internal class BitBuffer
	{
		public EndianType Endian { get; set; }
		public byte[] Data => _data.ToArray();
		public int Length => _data.Count;
		public int CurrentBit { get; private set; }
		public int CurrentByte => (CurrentBit - (CurrentBit % 8)) / 8;
		public int BitsLeft => (_data.Count * 8) - CurrentBit;
		public int BytesLeft => _data.Count - CurrentByte;

		private readonly List<byte> _data;

		public BitBuffer(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Value cannot be null.");

			_data = new List<byte>(data);
			Endian = EndianType.Little;
		}

		public void SeekBits(int count)
		{
			SeekBits(count, SeekOrigin.Current);
		}

		public void SeekBits(int offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Current:
					CurrentBit += offset;
					break;
				case SeekOrigin.Begin:
					CurrentBit = offset;
					break;
				case SeekOrigin.End:
					CurrentBit = (_data.Count * 8) - offset;
					break;
			}

			if (CurrentBit < 0 || CurrentBit > _data.Count * 8)
				throw new InvalidOperationException();
		}

		public void SeekBytes(int count)
		{
			SeekBits(count * 8);
		}

		public void SeekBytes(int offset, SeekOrigin origin)
		{
			SeekBits(offset * 8, origin);
		}

		public void SkipRemainingBits()
		{
			var bitOffset = CurrentBit % 8;
			if (bitOffset != 0)
				SeekBits(8 - bitOffset);
		}

		private uint ReadUnsignedBitsBigEndian(int nBits)
		{
			if (nBits <= 0 || nBits > 32)
				throw new ArgumentException("Value must be a positive integer between 1 and 32 inclusive.", nameof(nBits));
			if (CurrentBit + nBits > _data.Count * 8)
				throw new InvalidOperationException();

			var currentByte = CurrentBit / 8;
			var bitOffset = CurrentBit - (currentByte * 8);
			var nBytesToRead = (bitOffset + nBits) / 8;

			if ((bitOffset + nBits) % 8 != 0)
				nBytesToRead++;

			var currentValue = 0ul;
			for (var i = 0; i < nBytesToRead; i++)
				currentValue += (ulong)_data[currentByte + (nBytesToRead - 1) - i] << (i * 8);

			currentValue >>= (((nBytesToRead * 8) - bitOffset) - nBits);
			currentValue &= (uint)(((long)1 << nBits) - 1);

			CurrentBit += nBits;
			return (uint)currentValue;
		}

		private uint ReadUnsignedBitsLittleEndian(int nBits)
		{
			nBits = Math.Abs(nBits);
			if (nBits <= 0 || nBits > 32)
				throw new ArgumentException("Value must be a positive integer between 1 and 32 inclusive.", nameof(nBits));

			if (CurrentBit + nBits > _data.Count * 8)
				throw new InvalidOperationException();

			var currentByte = CurrentBit / 8;
			var bitOffset = CurrentBit - (currentByte * 8);
			var nBytesToRead = (bitOffset + nBits) / 8;

			if ((bitOffset + nBits) % 8 != 0)
				nBytesToRead++;

			var currentValue = 0ul;
			for (var i = 0; i < nBytesToRead; i++)
				currentValue += (ulong)_data[currentByte + i] << (i * 8);

			currentValue >>= bitOffset;
			currentValue &= (uint)(((long)1 << nBits) - 1);

			CurrentBit += nBits;
			return (uint)currentValue;
		}

		public uint ReadUnsignedBits(int nBits)
		{
			if (Endian == EndianType.Little)
				return ReadUnsignedBitsLittleEndian(nBits);
			return ReadUnsignedBitsBigEndian(nBits);
		}

		public int ReadBits(int nBits)
		{
			var result = (int)ReadUnsignedBits(nBits - 1);
			var sign = ReadBoolean() ? 1 : 0;
			if (sign == 1)
				result = -((1 << (nBits - 1)) - result);
			return result;
		}

		public bool ReadBoolean()
		{
			if (CurrentBit + 1 > _data.Count * 8)
				throw new InvalidOperationException();

			var result = (_data[CurrentBit / 8] & (Endian == EndianType.Little ? 1 << CurrentBit % 8 : 128 >> CurrentBit % 8)) != 0;
			CurrentBit++;
			return result;
		}

		public byte ReadByte()
		{
			return (byte)ReadUnsignedBits(8);
		}

		public sbyte ReadSByte()
		{
			return (sbyte)ReadBits(8);
		}

		public byte[] ReadBytes(int nBytes)
		{
			var result = new byte[nBytes];
			for (var i = 0; i < nBytes; i++)
				result[i] = ReadByte();
			return result;
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
			return (ushort)ReadUnsignedBits(16);
		}

		public int ReadInt32()
		{
			return ReadBits(32);
		}

		public uint ReadUInt32()
		{
			return ReadUnsignedBits(32);
		}

		public float ReadSingle()
		{
			return BitConverter.ToSingle(ReadBytes(4), 0);
		}

		public string ReadString(int length, bool ascii = false)
		{
			var startBit = CurrentBit;
			var s = ReadString(ascii);
			SeekBits((length * 8) - (CurrentBit - startBit));
			return s;
		}

		public string ReadString(bool ascii = false)
		{
			var bytes = new List<byte>();
			while (BytesLeft != 0)
			{
				var b = ReadByte();
				if (b == 0x00)
					break;
				bytes.Add(b);
			}
			return (ascii) ? Encoding.ASCII.GetString(bytes.ToArray()) : Encoding.UTF8.GetString(bytes.ToArray());
		}
	}
}