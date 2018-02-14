using System.Threading.Tasks;

namespace SourceDemoParser
{
	public abstract class NetMessage : INetMessage
	{
		public NetMessageType Type { get; set; }

		public NetMessage()
		{
		}
		public NetMessage(NetMessageType type)
		{
			Type = type;
		}

		public abstract Task Parse(ISourceBufferUtil buf, SourceDemo demo);
		public abstract Task Export(ISourceWriterUtil bw, SourceDemo demo);
	}
}