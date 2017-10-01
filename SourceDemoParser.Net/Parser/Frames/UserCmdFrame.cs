using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class UserCmdFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public int CmdNumber { get; set; }

		public UserCmdFrame()
		{
		}
		public UserCmdFrame(int cmd, byte[] data)
		{
			CmdNumber = cmd;
			RawData = data;
		}

		Task IFrame.ParseData()
		{
			// Todo
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = CmdNumber.GetBytes();
			RawData.GetBytes().AppendTo(ref bytes);
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> $"{CmdNumber}";
	}
}