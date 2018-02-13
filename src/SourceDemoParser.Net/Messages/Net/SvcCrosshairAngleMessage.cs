using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCrosshairAngleMessage : INetMessage
	{
		public QAngle Angle { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var x = buf.ReadSingle();
			var y = buf.ReadSingle();
			var z = buf.ReadSingle();
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}