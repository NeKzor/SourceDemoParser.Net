using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public delegate Task<IFrame> MessageParser(BinaryReader br, SourceDemo demo);
	public delegate Task MessageExporter(BinaryWriter bw, IFrame frame);

	public class DemoMessageType
	{
		public int Code { get; private set; }
		public string Name { get; private set; }

		internal MessageParser Parser { get; private set; }
		internal MessageExporter Exporter { get; private set; }

		public DemoMessageType(string name, MessageParser parser, MessageExporter exporter)
		{
			Name = name;
			Parser = parser;
			Exporter = exporter;
		}

		public DemoMessageType WithCode(int code)
		{
			Code = code;
			return this;
		}

		public override string ToString()
			=> Name;
	}
}