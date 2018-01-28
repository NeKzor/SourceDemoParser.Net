using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public delegate Task<IFrame> FrameHandler(BinaryReader br, SourceDemo demo);
    
    public class DemoMessageType
	{
		public int Type { get; private set; }
		public string Name { get; private set; }

		internal FrameHandler Handler { get; private set; }
		
		public DemoMessageType(string name, FrameHandler handler = null)
		{
			Name = name;
			Handler = handler;
		}

		public DemoMessageType WithCode(int code)
		{
			Type = code;
			return this;
		}
		
		public override string ToString()
			=> Name;
	}
}