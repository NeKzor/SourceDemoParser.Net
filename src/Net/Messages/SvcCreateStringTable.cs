using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcCreateStringTable : NetMessage
    {
        public string TableName { get; set; }
        public ushort MaxEntries { get; set; }
        public uint Entries { get; set; }
        public bool UserDataFixedSize { get; set; }
        public int UserDataSize { get; set; }
        public int UserDataSizeBits { get; set; }
        public int Flags { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            TableName = buf.ReadString();
            MaxEntries = buf.ReadUInt16();
            var bits = (int)System.Math.Log(MaxEntries, 2) + 1;
            Entries = bits != 1 ? buf.ReadUBits(bits) : buf.ReadUBits(1);
            var length = buf.ReadBits(20);
            UserDataFixedSize = buf.ReadBoolean();
            UserDataSize = (UserDataFixedSize) ? buf.ReadBits(12) : 0;
            UserDataSizeBits = (UserDataFixedSize) ? buf.ReadBits(4) : 0;
            Flags = buf.ReadBits(demo.Protocol == 4 ? 2 : 1);
            buf.SeekBits(length);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteString(TableName.AsSpan());
            buf.WriteUInt16(MaxEntries);
            buf.WriteUBits(Entries, (int)System.Math.Log(MaxEntries, 2) + 1);
            buf.WriteBits(Data.Length, 20);
            buf.WriteBoolean(UserDataFixedSize);
            if (UserDataFixedSize) buf.WriteBits(UserDataSize, 12);
            if (UserDataFixedSize) buf.WriteBits(UserDataSizeBits, 4);
            buf.WriteBits(Flags, demo.Protocol == 4 ? 2 : 1);
            return Task.CompletedTask;
        }
    }
}
