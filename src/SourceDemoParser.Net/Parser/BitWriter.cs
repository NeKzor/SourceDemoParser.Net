using System.Collections.Generic;
using System.Diagnostics;

namespace SourceDemoParser
{
	[DebuggerDisplay("{CurrentByte,nq}")]
	internal class BitWriter
	{
		public byte[] Data => _data.ToArray();
		public int CurrentBit => _currentBit;
		public int CurrentByte => _currentBit / 8;

		private readonly List<byte> _data;
		private int _currentBit;

		public BitWriter()
		{
			_data = new List<byte>();
		}
		public BitWriter(IEnumerable<byte> data) : this()
		{
			_data.AddRange(data);
		}

		public void WriteBits(int value, int count)
		{
			WriteUBits((uint)value, count - 1);
			WriteUBits((value < 0) ? 1u : 0u, 1);
		}
		public void WriteUBits(uint value, int count)
		{
			var currentByte = _currentBit / 8;
			var bitOffset = _currentBit - (currentByte * 8);

			var bitsToWriteToCurrentByte = 8 - bitOffset;
			if (bitsToWriteToCurrentByte > count)
				bitsToWriteToCurrentByte = count;

			var bytesToAdd = 0;
			if (count > bitsToWriteToCurrentByte)
			{
				var temp = count - bitsToWriteToCurrentByte;
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

			while (nBitsWritten < count)
			{
				bitsToWriteToCurrentByte = count - nBitsWritten;
				if (bitsToWriteToCurrentByte > 8)
					bitsToWriteToCurrentByte = 8;

				b = (byte)((value >> nBitsWritten) & ((1 << bitsToWriteToCurrentByte) - 1));
				_data[currentByte] = b;

				nBitsWritten += bitsToWriteToCurrentByte;
				currentByte++;
			}
			_currentBit += count;
		}

		public void WriteOneBit(int value)
		{
			WriteBits(value, 1);
		}
		public void WriteOneUBit(uint value)
		{
			WriteUBits(value, 1);
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
			WriteUBits(value, 8);
		}
		public void WriteSByte(sbyte value)
		{
			WriteBits(value, 8);
		}
		public void WriteBytes(IEnumerable<byte> values)
		{
			foreach (byte value in values)
				WriteByte(value);
		}
		public void WriteChars(IEnumerable<char> values)
		{
			foreach (char value in values)
				WriteByte((byte)value);
		}

		public void WriteInt16(short value)
		{
			WriteBits(value, 16);
		}
		public void WriteUInt16(ushort value)
		{
			WriteUBits(value, 16);
		}
		public void WriteInt32(int value)
		{
			WriteBits(value, 32);
		}
		public void WriteUInt32(uint value)
		{
			WriteUBits(value, 32);
		}

		public void WriteString(string value, bool withNullTerminator = true)
		{
			foreach (char letter in value)
				WriteByte((byte)letter);
			if (withNullTerminator) WriteByte(0);
		}
		public void WriteString(string value, int size)
		{
			WriteString(value, false);
			for (var i = 0; i < size - value.Length; i++)
				WriteByte(0);
		}
	}
}