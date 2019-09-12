using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SourceDemoParser.Engine;

namespace SourceDemoParser.Messages.Net
{
    public class SvcClassInfo : NetMessage
    {
        public bool CreateOnClient { get; set; }
        public List<ServerClassInfo> ServerClasses { get; set; }= new List<ServerClassInfo>();

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            var length = buf.ReadInt16();
            CreateOnClient = buf.ReadBoolean();
            if (!CreateOnClient)
            {
                while (length-- > 0)
                {
                    ServerClasses.Add(new ServerClassInfo()
                    {
                        ClassId = (short)buf.ReadBits((int)System.Math.Log(length, 2) + 1),
                        ClassName = buf.ReadString(),
                        DataTableName = buf.ReadString()
                    });
                }
            }
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            var length = (short)ServerClasses.Count;
            buf.WriteInt16(length);
            buf.WriteBoolean(CreateOnClient);
            if (!CreateOnClient)
            {
                foreach (var sclass in ServerClasses)
                {
                    --length;
                    buf.WriteBits(sclass.ClassId, (int)System.Math.Log(length, 2) + 1);
                    buf.WriteString(sclass.ClassName.AsSpan());
                    buf.WriteString(sclass.DataTableName.AsSpan());
                }
            }
            return Task.CompletedTask;
        }
    }
}
