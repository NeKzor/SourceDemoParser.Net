using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class CustomDataMessage : DemoMessage<CustomDataFrame>
	{
		public int Unknown { get; set; }

		public override Task Parse(BinaryReader br, SourceDemo demo)
		{
			Unknown = br.ReadInt32();
			Data= br.ReadBytes(br.ReadInt32());
			return Task.CompletedTask;
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write(Unknown);
			bw.Write(Data.Length);
			bw.Write(Data);
			return Task.CompletedTask;
		}
	}
}