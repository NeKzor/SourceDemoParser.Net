using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class NetSignonStateMessage : NetMessage
    {
        public byte SignonState { get; set; }
        public int SpawnCount { get; set; }
        public int NumServerPlayers { get; set; }
        public byte[] PlayerNetworkIds { get; set; }
        public string MapName { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
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
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteByte(SignonState);
            bw.WriteInt32(SpawnCount);
            if (demo.Protocol == 4)
            {
                bw.WriteInt32(NumServerPlayers);
                if (PlayerNetworkIds.Length > 0)
                {
                    bw.WriteInt32(PlayerNetworkIds.Length);
                    bw.WriteBytes(PlayerNetworkIds);
                }
                if (MapName.Length > 0)
                {
                    bw.WriteInt32(MapName.Length);
                    bw.WriteString(MapName);
                }
            }
            return Task.CompletedTask;
        }
    }
}
