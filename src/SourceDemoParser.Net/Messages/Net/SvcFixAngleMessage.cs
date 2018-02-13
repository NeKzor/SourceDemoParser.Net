using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcFixAngleMessage : INetMessage
	{
		public bool Relative { get; set; }
		public QAngle Angle { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var relative = buf.ReadBoolean();
			var x = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			var y = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			var z = (buf.ReadBoolean()) ? buf.ReadSingle() : 0;
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}