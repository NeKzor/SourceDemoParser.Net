using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Messages.Net
{
	public class SvcCrosshairAngleMessage : NetMessage
	{
		public QAngle Angle { get; set; }
		
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Angle = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle());
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			//bw.Single(Angle.X);
			//bw.Single(Angle.Y);
			//bw.Single(Angle.Z);
			return Task.CompletedTask;
		}
	}
}