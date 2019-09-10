using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
    public class SvcBspDecalMessage : NetMessage
    {
        public Vector Position { get; set; }
        public uint DecalTextureIndex { get; set; }
        public uint? EntityIndex { get; set; }
        public uint? ModelIndex { get; set; }
        public bool LowPriority { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
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
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteVectorBitCoord(Position);
            bw.WriteUBits(DecalTextureIndex, 9);
            if (EntityIndex.HasValue && ModelIndex.HasValue)
            {
                bw.WriteBoolean(true);
                bw.WriteUBits(EntityIndex.Value, 11);
                bw.WriteUBits(ModelIndex.Value, 11);
            }
            else
            {
                bw.WriteBoolean(false);
            }
            bw.WriteBoolean(LowPriority);
            return Task.CompletedTask;
        }
    }
}
