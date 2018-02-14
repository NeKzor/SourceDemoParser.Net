using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class StopMessage : DemoMessage<StopFrame>
	{
		public override Task Parse(BinaryReader br, SourceDemo demo)
		{
			var length = br.BaseStream.Length - br.BaseStream.Position;
			Data = (length > 0) ? br.ReadBytes((int)length) : default(byte[]);
			return Task.CompletedTask;
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			if (Data != null) bw.Write(Data);
			return Task.CompletedTask;
		}
	}
}