using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface ISourceParser
	{
		Task<SourceDemo> ParseAsync(Stream stream);
		Task<IFrame> HandleMessageAsync(BinaryReader br, IDemoMessage message);
	}
}