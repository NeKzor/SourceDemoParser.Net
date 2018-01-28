using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface ISourceExporter
	{
		Task ExportAsync(BinaryWriter bw, SourceDemo demo);
	}
}