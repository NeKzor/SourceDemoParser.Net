using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetSetConVarMessage : NetMessage
    {
        public List<ConVar> ConVars { get; set; }

        public NetSetConVarMessage()
        {
            ConVars = new List<ConVar>();
        }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            var size = buf.ReadBits(8);
            while (size-- != 0)
            {
                ConVars.Add(new ConVar()
                {
                    Name = buf.ReadString(),
                    Value = buf.ReadString()
                });
            }
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteBits(ConVars.Count, 8);
            foreach (var convar in ConVars)
            {
                bw.WriteString(convar.Name);
                bw.WriteString(convar.Value);
            }
            return Task.CompletedTask;
        }
    }
}
