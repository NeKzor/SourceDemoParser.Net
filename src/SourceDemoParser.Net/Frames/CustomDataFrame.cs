using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class CustomDataFrame : IDemoFrame
	{
		public byte[] Data { get; set; }
		public int Unknown1 { get; set; }
		public string Unknown2 { get; set; }

		Task IDemoFrame.Parse(SourceDemo demo)
		{
			var buf = new BitBuffer(Data);
			Unknown1 = buf.ReadInt32();
			Unknown2 = buf.ReadString();
			return Task.CompletedTask;
		}
		Task<byte[]> IDemoFrame.Export(SourceDemo demo)
		{
			var data = new byte[0];
			Unknown1.ToBytes().AppendTo(ref data);
			$"{Unknown2}\0".ToBytes(false).AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}