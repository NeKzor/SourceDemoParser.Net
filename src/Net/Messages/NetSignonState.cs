using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetSignonState : NetMessage
    {
        public byte SignonState { get; set; }
        public int SpawnCount { get; set; }
        public int NumServerPlayers { get; set; }
        public byte[] PlayerNetworkIds { get; set; }
        public string MapName { get; set; }

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
        {
            SignonState = buf.ReadByte();
            SpawnCount = buf.ReadInt32();
            if (demo.Protocol == 4)
            {
                NumServerPlayers = buf.ReadInt32();
                var length = buf.ReadInt32();
                if (length > 0)
                {
                    PlayerNetworkIds = buf.ReadBytes(length);
                }
                length = buf.ReadInt32();
                if (length > 0)
                {
                    MapName = buf.ReadString(length);
                }
            }
            return Task.CompletedTask;
        }
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteByte(SignonState);
            buf.WriteInt32(SpawnCount);
            if (demo.Protocol == 4)
            {
                buf.WriteInt32(NumServerPlayers);
                if (PlayerNetworkIds.Length > 0)
                {
                    buf.WriteInt32(PlayerNetworkIds.Length);
                    buf.WriteBytes(PlayerNetworkIds);
                }
                if (MapName.Length > 0)
                {
                    buf.WriteInt32(MapName.Length);
                    buf.WriteString(MapName.AsSpan());
                }
            }
            return Task.CompletedTask;
        }
    }
}
