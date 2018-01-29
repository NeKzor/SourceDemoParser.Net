using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface IFrame
	{
		Task ParseData(SourceDemo demo);
		Task<byte[]> ExportData();
	}
}