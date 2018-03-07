using SourceDemoParser.Messages.Net;

namespace SourceDemoParser.Types.Net
{
	public class SvcPaintmapData : NetMessageType<SvcPaintmapDataMessage>
	{
		public SvcPaintmapData(int code) : base(code)
		{
		}
	}
}