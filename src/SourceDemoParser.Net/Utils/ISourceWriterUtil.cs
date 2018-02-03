using System.Collections.Generic;

namespace SourceDemoParser
{
	public interface ISourceWriterUtil
	{	
		byte[] Data { get; }
		int CurrentBit { get; }
		int CurrentByte { get; }
		
		void WriteBits(int value, int count);
		void WriteUBits(uint value, int count);
		void WriteOneBit(int value);
		void WriteOneUBit(uint value);
		void WriteBoolean(bool value);
		void WriteByte(byte value);
		void WriteSByte(sbyte value);
		void WriteBytes(IEnumerable<byte> values);
		void WriteChars(IEnumerable<char> values);
		void WriteInt16(short value);
		void WriteUInt16(ushort value);
		void WriteInt32(int value);
		void WriteUInt32(uint value);
		void WriteString(string value, bool withNullTerminator = true);
		void WriteString(string value, int size);
	}
}