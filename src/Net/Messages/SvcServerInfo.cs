using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
    public class SvcServerInfo : NetMessage
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

        public override Task Read(SourceBufferReader buf, SourceDemo demo)
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
        public override Task Write(SourceBufferWriter buf, SourceDemo demo)
        {
            buf.WriteInt16(Protocol);
            buf.WriteInt32(ServerCount);
            buf.WriteBoolean(IsHltv);
            buf.WriteBoolean(IsDedicated);
            buf.WriteInt32(ClientCrc);
            buf.WriteUInt16(MaxClasses);
            buf.WriteInt32(MapCrc);
            buf.WriteByte(PlayerSlot);
            buf.WriteByte(MaxClients);
            if (Unk.HasValue)
            {
                if (demo.Protocol == 4)
                {
                    buf.WriteInt32(Unk.Value);
                }
                else if (demo.NetworkProtocol == 24)
                {
                    buf.WriteInt32(Unk.Value);
                }
            }
            buf.WriteString(GameDir.AsSpan());
            buf.WriteString(MapName.AsSpan());
            buf.WriteString(SkyName.AsSpan());
            buf.WriteString(HostName.AsSpan());
            return Task.CompletedTask;
        }
    }
}
