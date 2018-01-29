using System.Collections.Generic;

namespace SourceDemoParser.Extensions
{
	internal class AdjustmentCache
	{
		public object Instance { get; set; }
		public ISourceDemo Demo { get; set; }
		public string GameDirectory => Demo.GameDirectory;
		public uint DefaultTickrate => Demo.DefaultTickrate;
		public IEnumerable<Adjustment> Adjustments { get; set; }
	}
}