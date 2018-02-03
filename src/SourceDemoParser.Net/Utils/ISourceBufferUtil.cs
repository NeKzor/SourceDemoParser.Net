using System.IO;

namespace SourceDemoParser
{
	public interface ISourceBufferUtil
	{	
		byte[] Data { get; }
		int Length { get; }
		int CurrentBit { get; }
		int CurrentByte { get; }
		int BitsLeft { get; }
		int BytesLeft { get; }
		
		void SeekBits(int count, SeekOrigin origin = SeekOrigin.Current);
		void SeekBytes(int count, SeekOrigin origin = SeekOrigin.Current);
		void SkipBits();
		uint ReadUBits(int count);
		int ReadBits(int count);
		uint ReadOneUBit();
		int ReadOneBit();
		bool ReadBoolean();
		byte ReadByte();
		sbyte ReadSByte();
		byte[] ReadBytes(int count);
		char ReadChar();
		char[] ReadChars(int nChars);
		short ReadInt16();
		ushort ReadUInt16();
		int ReadInt32();
		uint ReadUInt32();
		float ReadSingle();
		string ReadString(int length, bool encodeToAscii = true);
		string ReadString(bool encodeToAscii = true);
	}
}