using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class SyncTickMessage : DemoMessage
	{
		public override Task Parse(BinaryReader br, SourceDemo demo)
			=> Task.CompletedTask;
		public override Task Export(BinaryWriter bw, SourceDemo demo)
			=> Task.CompletedTask;
	}
}