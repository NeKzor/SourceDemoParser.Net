using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class SvcBspDecalMessage : INetMessage
	{
		public Vector Position { get; set; }
		public uint DecalTextureIndex { get; set; }
		public bool HasEntities { get; set; }
		public uint EntityIndex { get; set; }
		public uint ModelIndex { get; set; }
		public bool LowPriority { get; set; }
	}
}