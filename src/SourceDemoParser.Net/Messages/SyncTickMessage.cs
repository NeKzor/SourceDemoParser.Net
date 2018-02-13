using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages
{
	public class SyncTickMessage : DemoMessage
	{
		public SyncTickMessage(DemoMessageType type) : base(type)
		{
		}
	}
}