using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public interface INetMessage
	{
		/* Task ParseData(SourceDemo demo);
		Task<byte[]> ExportData(); */
	}
	
	public delegate Task<INetMessage> NetDataParser(ISourceBufferUtil buf, SourceDemo demo);
	public delegate Task NetDataExporter(ISourceWriterUtil bw, INetMessage frame);

	public class NetMessageType
	{
		public int Code { get; private set; }
		public string Name { get; private set; }

		public static readonly NetMessageType Empty = new NetMessageType(null, null, null);

		internal NetDataParser Parser { get; private set; }
		internal NetDataExporter Exporter { get; private set; }

		public NetMessageType(string name, NetDataParser parser, NetDataExporter exporter)
		{
			Name = name;
			Parser = parser;
			Exporter = exporter;
		}

		public NetMessageType WithCode(int code)
		{
			Code = code;
			return this;
		}

		public override string ToString()
			=> Name;
	}
}