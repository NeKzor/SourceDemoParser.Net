using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface INetMessage
	{
		NetMessageType Type { get; set; }
		Task Parse(ISourceBufferUtil buf, SourceDemo demo);
		Task Export(ISourceWriterUtil bw, SourceDemo demo);
	}
}