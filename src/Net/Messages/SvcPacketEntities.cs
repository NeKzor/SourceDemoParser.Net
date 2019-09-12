using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcPacketEntities : NetMessage
    {
        public int MaxEntries { get; set; }
        public bool IsDelta { get; set; }
        public int DeltaFrom { get; set; }
        public bool BaseLine { get; set; }
        public int UpdatedEntries { get; set; }
        public bool UpdateBaseline { get; set; }
        public byte[] Data { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            MaxEntries = buf.ReadBits(11);
            IsDelta = buf.ReadBoolean();
            DeltaFrom = (IsDelta) ? buf.ReadInt32() : 0;
            BaseLine = buf.ReadBoolean();
            UpdatedEntries = buf.ReadBits(11);
            var length = buf.ReadBits(20);
            UpdateBaseline = buf.ReadBoolean();
            buf.SeekBits(length);
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(MaxEntries, 11);
            buf.WriteBoolean(IsDelta);
            if (IsDelta) buf.WriteInt32(DeltaFrom);
            buf.WriteBoolean(BaseLine);
            buf.WriteBits(UpdatedEntries, 11);
            buf.WriteBits(Data.Length, 20);
            buf.WriteBoolean(UpdateBaseline);
            buf.WriteBytes(Data);
            return Task.CompletedTask;
        }
    }
}
