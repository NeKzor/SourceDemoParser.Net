using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class StopMessage : DemoMessage
	{
		public StopMessage(DemoMessageType type) : base(type)
		{
		}
	}
}