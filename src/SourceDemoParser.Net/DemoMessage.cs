using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class DemoMessage : IDemoMessage
	{
		public DemoMessageType Type { get; }
		public int Tick { get; set; }
		public IDemoFrame Frame { get; set; }

		public DemoMessage(DemoMessageType type)
			=> Type = type;

		public virtual Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
			=> Task.FromResult(default(IDemoFrame));
		public virtual Task Export(BinaryWriter bw, SourceDemo demo)
			=> Task.CompletedTask;

		public override string ToString()
			=> $"[{Tick}] 0x{Type.MessageType.ToString("X")} as {Type}";
	}
}