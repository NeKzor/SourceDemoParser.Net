using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class PacketMessage : DemoMessage<PacketFrame>
	{
		public override Task Parse(BinaryReader br, SourceDemo demo)
		{
			var temp = br.ReadBytes(((demo.Game.MaxSplitscreenClients ?? 1) * 76) + 8);
			br.ReadBytes(br.ReadInt32()).AppendTo(ref temp);
			Data = temp;
			return Task.CompletedTask;
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			var length = ((demo.Game.MaxSplitscreenClients ?? 1) * 76) + 8;
			bw.Write(Data.Take(length).ToArray());
			bw.Write(Data.Length - length);
			bw.Write(Data.Skip(length).ToArray());
			return Task.CompletedTask;
		}
	}
}