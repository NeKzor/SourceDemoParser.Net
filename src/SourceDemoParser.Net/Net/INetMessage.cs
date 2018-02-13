using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface INetMessage
	{
		Task Parse(ISourceBufferUtil buf, SourceDemo demo);
		Task Export(ISourceWriterUtil bw, SourceDemo demo);
	}
}