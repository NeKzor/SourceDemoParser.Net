using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages.Net
{
    public class SvcBspDecal : NetMessage
    {
        public Vector Position { get; set; }
        public uint DecalTextureIndex { get; set; }
        public uint? EntityIndex { get; set; }
        public uint? ModelIndex { get; set; }
        public bool LowPriority { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Position = buf.ReadVectorBitCoord();
            DecalTextureIndex = buf.ReadUBits(9);
            if (buf.ReadBoolean())
            {
                EntityIndex = buf.ReadUBits(11);
                ModelIndex = buf.ReadUBits(11);
            }
            LowPriority = buf.ReadBoolean();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteVectorBitCoord(Position);
            buf.WriteUBits(DecalTextureIndex, 9);
            if (EntityIndex.HasValue && ModelIndex.HasValue)
            {
                buf.WriteBoolean(true);
                buf.WriteUBits(EntityIndex.Value, 11);
                buf.WriteUBits(ModelIndex.Value, 11);
            }
            else
            {
                buf.WriteBoolean(false);
            }
            buf.WriteBoolean(LowPriority);
            return Task.CompletedTask;
        }
    }
}
