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
		public bool AutoConfiguration { get; set; }

		protected SourceParserBase(ParsingMode mode, AdjustmentType autoAdjustment, bool autoConfiguration)
		{
			Mode = mode;
			AutoAdjustment = autoAdjustment;
			AutoConfiguration = autoConfiguration;
		}

		public abstract Task<SourceDemo> ParseAsync(Stream input);
		public abstract Task<SourceDemo> ParseHeader(BinaryReader br, SourceDemo demo);
		public abstract Task Configure(SourceDemo demo);
	}
}