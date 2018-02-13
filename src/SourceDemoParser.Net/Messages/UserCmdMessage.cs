using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class UserCmdMessage : DemoMessage
	{
		public UserCmdMessage(DemoMessageType type) : base(type)
		{
		}

		public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
		{
			var cmd = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);
			return Task.FromResult(Frame = new UserCmdFrame(cmd, data) as IDemoFrame);
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write((Frame as UserCmdFrame).CmdNumber);
			bw.Write((Frame as UserCmdFrame).RawData.Length);
			bw.Write((Frame as UserCmdFrame).RawData);
			return Task.CompletedTask;
		}
	}
}