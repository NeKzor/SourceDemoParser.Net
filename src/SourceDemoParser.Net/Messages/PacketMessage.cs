using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class PacketMessage : DemoMessage
	{
		public PacketMessage(DemoMessageType type) : base(type)
		{
		}

		public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
		{
			var info = br.ReadBytes(((demo.Game.MaxSplitscreenClients ?? 1) * 76) + 8);
			var length = br.ReadInt32();
			var net = br.ReadBytes(length);
			return Task.FromResult(Frame = new PacketFrame(info, net) as IDemoFrame);
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write((Frame as PacketFrame).PacketData);
			bw.Write((Frame as PacketFrame).NetData.Length);
			bw.Write((Frame as PacketFrame).NetData);
			return Task.CompletedTask;
		}
	}
}