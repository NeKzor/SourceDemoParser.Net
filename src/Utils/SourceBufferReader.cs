using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using SourceDemoParser.Engine;

namespace SourceDemoParser
{
    // Thanks Traderain
    [DebuggerDisplay("{CurrentByte,nq}/{Length,nq}")]
    public class SourceBufferReader
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

        public SourceBufferReader(byte[] data)
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
            ++_currentBit;
            return result;
        }

        public T Read<T>()
        {
            if (typeof(T) == typeof(int))
                return (T)Convert.ChangeType(ReadInt32(), typeof(T));
            if (typeof(T) == typeof(float))
                return (T)Convert.ChangeType(ReadSingle(), typeof(T));
            if (typeof(T) == typeof(byte))
                return (T)Convert.ChangeType(ReadByte(), typeof(T));
            if (typeof(T) == typeof(short))
                return (T)Convert.ChangeType(ReadInt16(), typeof(T));
            if (typeof(T) == typeof(Vector))
                return (T)Convert.ChangeType(new Vector(ReadSingle(), ReadSingle(), ReadSingle()), typeof(T));
            if (typeof(T) == typeof(QAngle))
                return (T)Convert.ChangeType(new QAngle(ReadSingle(), ReadSingle(), ReadSingle()), typeof(T));
            throw new Exception($"Type {typeof(T)} not supported.");
        }
        public T ReadBits<T>(int bits)
        {
            if (typeof(T) == typeof(int?))
                return (T)Convert.ChangeType(ReadBits(bits), typeof(T));
            throw new Exception($"Type {typeof(T)} not supported.");
        }
        public T ReadField<T>(Action fieldSetCallback = null)
        {
            var result = default(T);
            if (ReadBoolean())
            {
                if (typeof(T) == typeof(int))
                    result = (T)Convert.ChangeType(ReadInt32(), typeof(T));
                else if (typeof(T) == typeof(float))
                    result = (T)Convert.ChangeType(ReadSingle(), typeof(T));
                else if (typeof(T) == typeof(byte))
                    result = (T)Convert.ChangeType(ReadByte(), typeof(T));
                else if (typeof(T) == typeof(short))
                    result = (T)Convert.ChangeType(ReadInt16(), typeof(T));
                else
                    throw new Exception($"Type {typeof(T)} not supported.");

                if (fieldSetCallback != null)
                    fieldSetCallback.Invoke();
            }

            return result;
        }
        public T ReadBitField<T>(int bits, Action fieldSetCallback = null)
        {
            var result = default(T);
            if (ReadBoolean())
            {
                if (typeof(T) == typeof(int))
                    result = (T)Convert.ChangeType(ReadBits(bits), typeof(T));
                else
                    throw new Exception($"Type {typeof(T)} not supported.");

                if (fieldSetCallback != null)
                    fieldSetCallback.Invoke();
            }

            return result;
        }
        public string ReadStringField()
        {
            return ReadString(ReadInt32());
        }
        public byte[] ReadBufferField()
        {
            return ReadBytes(ReadInt32());
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
        public float ReadBitCoord()
        {
            const int COORD_INTEGER_BITS = 14;
            const int COORD_FRACTIONAL_BITS = 5;
            const int COORD_DENOMINATOR = 1 << COORD_FRACTIONAL_BITS;
            const float COORD_RESOLUTION = 1.0f / COORD_DENOMINATOR;

            var value = 0.0f;
            var intval = ReadBits(1);
            var fractval = ReadBits(1);
            if (intval != 0 || fractval != 0)
            {
                var signbit = ReadBits(1);
                if (intval != 0)
                {
                    intval = ReadBits(COORD_INTEGER_BITS) + 1;
                }
                if (fractval != 0)
                {
                    fractval = ReadBits(COORD_FRACTIONAL_BITS);
                }
                value = intval + fractval * COORD_RESOLUTION;
                if (signbit != 0) value = -value;
            }

            return value;
        }
        public Vector ReadVectorBitCoord()
        {
            var x = 0.0f; var y = 0.0f; var z = 0.0f;
            if (ReadBoolean())
                x = ReadBitCoord();
            if (ReadBoolean())
                y = ReadBitCoord();
            if (ReadBoolean())
                z = ReadBitCoord();
            return new Vector(x, y, z);
        }

        public string ReadString(int length, bool encodeToAscii = true)
        {
            Span<byte> str = (length <= 1024) ? stackalloc byte[length] : new byte[length];

            var idx = 0;
            while ((Length - CurrentByte >= 1) && (length > 0))
            {
                str[idx] = ReadByte();
                ++idx;
                --length;
            }

            return (encodeToAscii)
                ? Encoding.ASCII.GetString(str.ToArray())
                : Encoding.UTF8.GetString(str.ToArray());
        }
        public string ReadString(bool encodeToAscii = true)
        {
            var str = new List<byte>();
            while (Length - CurrentByte >= 1)
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
                ++nBytesToRead;

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
                ++nBytesToRead;

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
