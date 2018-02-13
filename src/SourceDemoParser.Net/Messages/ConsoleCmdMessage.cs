using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class ConsoleCmdMessage : DemoMessage
	{
		public ConsoleCmdMessage(DemoMessageType type) : base(type)
		{
		}

		public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(Frame = new ConsoleCmdFrame(data) as IDemoFrame);
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write((Frame as ConsoleCmdFrame).RawData.Length);
			bw.Write((Frame as ConsoleCmdFrame).RawData);
			return Task.CompletedTask;
		}
	}
}