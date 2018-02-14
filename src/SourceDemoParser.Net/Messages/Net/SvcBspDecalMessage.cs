using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcBspDecalMessage : NetMessage
	{
		public Vector Position { get; set; }
		public uint DecalTextureIndex { get; set; }
		public bool HasEntities { get; set; }
		public uint EntityIndex { get; set; }
		public uint ModelIndex { get; set; }
		public bool LowPriority { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Position = new Vector(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle());
			DecalTextureIndex = buf.ReadUInt32();
			HasEntities = buf.ReadBoolean();
			EntityIndex = buf.ReadUInt32();
			ModelIndex = buf.ReadUInt32();
			LowPriority = buf.ReadBoolean();
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			//bw.Single(Position.X);
			//bw.Single(Position.Y);
			//bw.Single(Position.Z);
			bw.WriteUInt32(DecalTextureIndex);
			bw.WriteBoolean(HasEntities);
			bw.WriteUInt32(EntityIndex);
			bw.WriteUInt32(ModelIndex);
			bw.WriteBoolean(LowPriority);
			return Task.CompletedTask;
		}
	}
}