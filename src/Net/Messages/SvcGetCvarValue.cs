using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcGetCvarValue : NetMessage
    {
        public int Cookie { get; set; }
        public string CvarName { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            Cookie = buf.ReadInt32();
            CvarName = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt32(Cookie);
            buf.WriteString(CvarName.AsSpan());
            return Task.CompletedTask;
        }
    }
}
