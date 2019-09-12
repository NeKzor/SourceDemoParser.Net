using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages
{
    public class UserCmd : DemoMessage
    {
        public int CmdNumber { get; set; }
        public SourceBufferReader Buffer { get; set; }
        public UserCmdInfo Cmd { get; set; } = new UserCmdInfo();

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            CmdNumber = buf.Read<int>();
            Buffer = new SourceBufferReader(buf.ReadBufferField());
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.Write(CmdNumber);
            buf.WriteBufferield(Buffer.Data);
            return Task.CompletedTask;
        }
    }
}
