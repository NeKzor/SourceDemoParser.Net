using System.Threading.Tasks;

namespace SourceDemoParser
{
	public abstract class NetMessage : INetMessage
	{
		public NetMessageType Type { get; }

		public NetMessage(NetMessageType type)
			=> type = Type;

		public abstract Task Parse(ISourceBufferUtil buf, SourceDemo demo);
		public abstract Task Export(ISourceWriterUtil bw, SourceDemo demo);
	}
}