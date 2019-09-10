using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcServerInfoMessage : NetMessage
    {
        public short Protocol { get; set; }
        public int ServerCount { get; set; }
        public bool IsHltv { get; set; }
        public bool IsDedicated { get; set; }
        public int ClientCrc { get; set; }
        public ushort MaxClasses { get; set; }
        public int MapCrc { get; set; }
        public byte PlayerSlot { get; set; }
        public byte MaxClients { get; set; }
        public int? Unk { get; set; }
        public float TickInterval { get; set; }
        public char OperatingSystem { get; set; }
        public string GameDir { get; set; }
        public string MapName { get; set; }
        public string SkyName { get; set; }
        public string HostName { get; set; }

        public override Task Parse(SourceBufferReader buf, SourceDemo demo)
        {
            Protocol = buf.ReadInt16();
            ServerCount = buf.ReadInt32();
            IsHltv = buf.ReadBoolean();
            IsDedicated = buf.ReadBoolean();
            ClientCrc = buf.ReadInt32();
            MaxClasses = buf.ReadUInt16();
            MapCrc = buf.ReadInt32();
            PlayerSlot = buf.ReadByte();
            MaxClients = buf.ReadByte();
            if (demo.Protocol == 4)
            {
                Unk = buf.ReadInt32();
            }
            else if (demo.NetworkProtocol == 24)
            {
                Unk = buf.ReadBits(96);
            }
            TickInterval = buf.ReadSingle();
            OperatingSystem = buf.ReadChar();
            GameDir = buf.ReadString();
            MapName = buf.ReadString();
            SkyName = buf.ReadString();
            HostName = buf.ReadString();
            return Task.CompletedTask;
        }
        public override Task Export(SourceBufferWriter bw, SourceDemo demo)
        {
            bw.WriteInt16(Protocol);
            bw.WriteInt32(ServerCount);
            bw.WriteBoolean(IsHltv);
            bw.WriteBoolean(IsDedicated);
            bw.WriteInt32(ClientCrc);
            bw.WriteUInt16(MaxClasses);
            bw.WriteInt32(MapCrc);
            bw.WriteByte(PlayerSlot);
            bw.WriteByte(MaxClients);
            if (Unk.HasValue)
            {
                if (demo.Protocol == 4)
                {
                    bw.WriteInt32(Unk.Value);
                }
                else if (demo.NetworkProtocol == 24)
                {
                    bw.WriteInt32(Unk.Value);
                }
            }
            bw.WriteString(GameDir);
            bw.WriteString(MapName);
            bw.WriteString(SkyName);
            bw.WriteString(HostName);
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{Type.Name}\n"
                + $"{nameof(Protocol)} -> {Protocol}\n"
                + $"{nameof(ServerCount)} -> {ServerCount}\n"
                + $"{nameof(IsHltv)} -> {IsHltv}\n"
                + $"{nameof(IsDedicated)} -> {IsDedicated}\n"
                + $"{nameof(ClientCrc)} -> {ClientCrc}\n"
                + $"{nameof(MaxClasses)} -> {MaxClasses}\n"
                + $"{nameof(MapCrc)} -> {MapCrc}\n"
                + $"{nameof(PlayerSlot)} -> {PlayerSlot}\n"
                + $"{nameof(MaxClients)} -> {MaxClients}\n"
                + $"{nameof(TickInterval)} -> {TickInterval}\n"
                + $"{nameof(OperatingSystem)} -> {OperatingSystem}\n"
                + $"{nameof(GameDir)} -> {GameDir}\n"
                + $"{nameof(MapName)} -> {MapName}\n"
                + $"{nameof(SkyName)} -> {SkyName}\n"
                + $"{nameof(HostName)} -> {HostName}";
        }
    }
}
