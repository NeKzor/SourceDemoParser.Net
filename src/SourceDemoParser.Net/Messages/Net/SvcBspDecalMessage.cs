using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcBspDecalMessage : INetMessage
	{
		public Vector Position { get; set; }
		public uint DecalTextureIndex { get; set; }
		public bool HasEntities { get; set; }
		public uint EntityIndex { get; set; }
		public uint ModelIndex { get; set; }
		public bool LowPriority { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var x = buf.ReadSingle();
			var y = buf.ReadSingle();
			var z = buf.ReadSingle();
			var texture = buf.ReadUInt32();
			var entities = buf.ReadBoolean();
			var entity = buf.ReadUInt32();
			var model = buf.ReadUInt32();
			var priority = buf.ReadBoolean();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}