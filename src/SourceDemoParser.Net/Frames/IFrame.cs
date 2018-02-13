using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface IDemoFrame
	{
		Task Parse(SourceDemo demo);
		Task<byte[]> Export();
	}
}