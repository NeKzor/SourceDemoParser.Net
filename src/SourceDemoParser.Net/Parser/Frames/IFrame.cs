using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface IFrame
	{
		byte[] RawData { get; set; }
		Task ParseData();
		Task<byte[]> ExportData();
		string ToString();
	}
}