using System;
using System.Collections.Generic;

namespace SourceDemoParser
{
	internal class BitWriter
	{
		public byte[] Data => _data.ToArray();

		private readonly List<byte> _data;
		private int _currentBit;

		public BitWriter()
		{
			_data = new List<byte>();
		}

		public void WriteUnsignedBits(uint value, int nBits)
		{
			var currentByte = _currentBit / 8;
			var bitOffset = _currentBit - (currentByte * 8);

			var bitsToWriteToCurrentByte = 8 - bitOffset;
			if (bitsToWriteToCurrentByte > nBits)
				bitsToWriteToCurrentByte = nBits;

			var bytesToAdd = 0;
			if (nBits > bitsToWriteToCurrentByte)
			{
				var temp = nBits - bitsToWriteToCurrentByte;
				bytesToAdd = temp / 8;

				if ((temp % 8) != 0)
					bytesToAdd++;
			}

			if (bitOffset == 0)
				bytesToAdd++;

			for (var i = 0; i < bytesToAdd; i++)
				_data.Add(new byte());

			var nBitsWritten = 0;
			var b = (byte)(value & ((1 << bitsToWriteToCurrentByte) - 1));
			b <<= bitOffset;
			b += _data[currentByte];
			_data[currentByte] = b;

			nBitsWritten += bitsToWriteToCurrentByte;
			currentByte++;

			while (nBitsWritten < nBits)
			{
				bitsToWriteToCurrentByte = nBits - nBitsWritten;
				if (bitsToWriteToCurrentByte > 8)
					bitsToWriteToCurrentByte = 8;

				b = (byte)((value >> nBitsWritten) & ((1 << bitsToWriteToCurrentByte) - 1));
				_data[currentByte] = b;

				nBitsWritten += bitsToWriteToCurrentByte;
				currentByte++;
			}

			_currentBit += nBits;
		}

		public void WriteOneBit(int value)
		{
			WriteBits(value, 1);
		}

		public void WriteOneUBit(uint value)
		{
			WriteUnsignedBits(value, 1);
		}

		public void WriteBits(int value, int nBits)
		{
			WriteUnsignedBits((uint)value, nBits - 1);
			var sign = (value < 0 ? 1u : 0u);
			WriteUnsignedBits(sign, 1);
		}

		public void WriteBoolean(bool value)
		{
			var currentByte = _currentBit / 8;
			if (currentByte > _data.Count - 1)
				_data.Add(new byte());

			if (value)
				_data[currentByte] += (byte)(1 << _currentBit % 8);
			_currentBit++;
		}

		public void WriteByte(byte value)
		{
			WriteUnsignedBits(value, 8);
		}

		public void WriteSByte(sbyte value)
		{
			WriteBits(value, 8);
		}

		public void WriteBytes(byte[] values)
		{
			foreach (byte t in values)
				WriteByte(t);
		}

		public void WriteChars(char[] values)
		{
			foreach (char t in values)
				WriteByte((byte)t);
		}

		public void WriteInt16(short value)
		{
			WriteBits(value, 16);
		}

		public void WriteUInt16(ushort value)
		{
			WriteUnsignedBits(value, 16);
		}

		public void WriteInt32(int value)
		{
			WriteBits(value, 32);
		}

		public void WriteUInt32(uint value)
		{
			WriteUnsignedBits(value, 32);
		}

		public void WriteString(string value)
		{
			foreach (char t in value)
				WriteByte((byte)t);
			WriteByte(0);
		}

		public void WriteString(string value, int length)
		{
			if (length < value.Length + 1)
				throw new Exception("String length is longer than specified length.");

			WriteString(value);
			for (var i = 0; i < length - (value.Length + 1); i++)
				WriteByte(0);
		}
	}
}