using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public delegate Task<IFrame> FrameParser(BinaryReader br, SourceDemo demo);
	public delegate Task FrameExporter(BinaryWriter bw, IFrame frame);
    
    public class DemoMessageType
	{
		public int Code { get; private set; }
		public string Name { get; private set; }

		internal FrameParser Parser { get; private set; }
		internal FrameExporter Exporter { get; private set; }

		public DemoMessageType(string name, FrameParser parser, FrameExporter exporter)
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