using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class CustomDataMessage : DemoMessage
	{
		public CustomDataMessage(DemoMessageType type) : base(type)
		{
		}

		public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
		{
			var idk = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(Frame = new CustomDataFrame(idk, data) as IDemoFrame);
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write((Frame as CustomDataFrame).Unknown1);
			bw.Write((Frame as CustomDataFrame).RawData.Length);
			bw.Write((Frame as CustomDataFrame).RawData);
			return Task.CompletedTask;
		}
	}
}