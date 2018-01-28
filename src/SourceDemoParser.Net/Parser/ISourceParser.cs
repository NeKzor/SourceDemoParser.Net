using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface ISourceParser
	{
		Task<SourceDemo> ParseAsync(Stream stream);
		Task<SourceDemo> ParseHeader(BinaryReader br, SourceDemo demo);
		Task Configure(SourceDemo demo);
	}
}