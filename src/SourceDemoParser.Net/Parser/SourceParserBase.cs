using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public abstract class SourceParserBase
	{
		public ParsingMode Mode { get; set; }
		public AdjustmentType AutoAdjustment { get; set; }
		public Func<SourceDemo, SourceGame> ConfigBuilder { get; set; }

		protected SourceParserBase(
			ParsingMode mode,
			AdjustmentType autoAdjustment,
			Func<SourceDemo, SourceGame> configBuilder)
		{
			Mode = mode;
			AutoAdjustment = autoAdjustment;
			ConfigBuilder = configBuilder ?? SourceGameBuilder.Default;
		}

		public abstract Task<SourceDemo> ParseAsync(Stream input);
		public abstract Task<SourceDemo> ParseHeader(BinaryReader br, SourceDemo demo);
	}
}