using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class UserCmdMessage : DemoMessage<UserCmdFrame>
	{
		public int CmdNumber { get; set; }

		public override Task Parse(BinaryReader br, SourceDemo demo)
		{
			CmdNumber = br.ReadInt32();
			Data = br.ReadBytes(br.ReadInt32());
			return Task.CompletedTask;
		}
		public override Task Export(BinaryWriter bw, SourceDemo demo)
		{
			bw.Write(CmdNumber);
			bw.Write(Data.Length);
			bw.Write(Data);
			return Task.CompletedTask;
		}
	}
}