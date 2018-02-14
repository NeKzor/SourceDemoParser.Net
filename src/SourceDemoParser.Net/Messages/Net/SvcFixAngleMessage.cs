using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcFixAngleMessage : NetMessage
	{
		public bool Relative { get; set; }
		public QAngle Angle { get; set; }

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Relative = buf.ReadBoolean();
			Angle = new QAngle
			(
				(buf.ReadBoolean()) ? buf.ReadSingle() : 0,
				(buf.ReadBoolean()) ? buf.ReadSingle() : 0,
				(buf.ReadBoolean()) ? buf.ReadSingle() : 0
			);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteBoolean(Relative);
			//bw.Single(Angle.X);
			//bw.Single(Angle.Y);
			//bw.Single(Angle.Z);
			return Task.CompletedTask;
		}
	}
}