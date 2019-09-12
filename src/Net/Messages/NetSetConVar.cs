using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages.Net
{
    public class NetSetConVar : NetMessage
    {
        public List<ConVar> ConVars { get; set; }= new List<ConVar>();

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
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
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteBits(ConVars.Count, 8);
            foreach (var convar in ConVars)
            {
                buf.WriteString(convar.Name.AsSpan());
                buf.WriteString(convar.Value.AsSpan());
            }
            return Task.CompletedTask;
        }
    }
}
