using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class DataTablesMessage : DemoMessage
	{
		public DataTablesMessage(DemoMessageType type) : base(type)
		{
		}

		public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(Frame = new DataTablesFrame(data) as IDemoFrame);
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write((Frame as DataTablesFrame).RawData.Length);
			bw.Write((Frame as DataTablesFrame).RawData);
			return Task.CompletedTask;
		}
	}
}