using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface IDemoMessage
	{
		DemoMessageType Type { get; }
		int Tick { get; set; }
		IDemoFrame Frame { get; set; }

		Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo);
		Task Export(BinaryWriter bw, SourceDemo demo);
	}
}