using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class ConsoleCmdMessage : DemoMessage<ConsoleCmdFrame>
	{
		public override Task Parse(BinaryReader br, SourceDemo demo)
		{
			Data = br.ReadBytes(br.ReadInt32());
			return Task.CompletedTask;
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write(Data.Length);
			bw.Write(Data);
			return Task.CompletedTask;
		}
	}
}