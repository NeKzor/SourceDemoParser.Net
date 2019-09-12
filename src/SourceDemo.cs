using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;
using SourceDemoParser.Messages;

namespace SourceDemoParser
{
    public class SourceDemo
    {
        public string HeaderId { get; set; }
        public int Protocol { get; set; }
        public int NetworkProtocol { get; set; }
        public string ServerName { get; set; }
        public string ClientName { get; set; }
        public string MapName { get; set; }
        public string GameDirectory { get; set; }
        public float PlaybackTime { get; set; }
        public int PlaybackTicks { get; set; }
        public int PlaybackFrames { get; set; }
        public int SignOnLength { get; set; }
        public List<IDemoMessage> Messages { get; set; } = new List<IDemoMessage>();
        public SourceGame Game { get; set; }

        public virtual Task ReadHeader(SourceBufferReader buf)
        {
            HeaderId = buf.ReadString(8);
            if (HeaderId != "HL2DEMO\0")
                throw new SourceException(HeaderId);

            Protocol = buf.Read<int>();
            NetworkProtocol = buf.Read<int>();
            ServerName = buf.ReadString(260).TrimEnd('\0');
            ClientName = buf.ReadString(260).TrimEnd('\0');
            MapName = buf.ReadString(260).TrimEnd('\0');
            GameDirectory = buf.ReadString(260).TrimEnd('\0');
            PlaybackTime = buf.Read<float>();
            PlaybackTicks = buf.Read<int>();
            PlaybackFrames = buf.Read<int>();
            SignOnLength = buf.Read<int>();

            return Task.CompletedTask;
        }

        public virtual async Task ReadMessagesAsync(SourceBufferReader buf)
        {
            var newEngine = Game.MaxSplitscreenClients.HasValue;
            var minBytesLeft = newEngine ? 6 : 5;

            while (buf.BytesLeft > minBytesLeft)
            {
                var code = buf.ReadByte();
                var type = Game.DefaultMessages[code];

                if (type == null)
                    throw new MessageTypeException(code, buf.CurrentByte);

                var message = (DemoMessage)Activator.CreateInstance(type);

                message.Code = code;
                message.Tick = buf.Read<int>();
                message.Slot = newEngine ? buf.Read<byte>() : default;

                await message.Read(buf, this).ConfigureAwait(false);

                Messages.Add(message);
            }
        }

        public virtual async Task ReadPacketsAsync()
        {
            foreach (var message in Messages)
            {
                if (message is Packet pa)
                    await this.Read(pa).ConfigureAwait(false);
            }
        }
        public virtual async Task ReadStringTablesAsync()
        {
            foreach (var message in Messages)
            {
                if (message is StringTables st)
                    await this.Read(st).ConfigureAwait(false);
            }
        }
        public virtual async Task ReadDataTablesAsync()
        {
            foreach (var message in Messages)
            {
                if (message is DataTables dt)
                    await this.Read(dt).ConfigureAwait(false);
            }
        }
        public virtual async Task ReadUserCmdsAsync()
        {
            foreach (var message in Messages)
            {
                if (message is UserCmd uc)
                    await this.Read(uc).ConfigureAwait(false);
            }
        }

        public virtual Task WriteHeader(SourceBufferWriter buf)
        {
            buf.Write(HeaderId);
            buf.Write(Protocol);
            buf.Write(NetworkProtocol);
            buf.WriteString(ServerName.AsSpan(), 260);
            buf.WriteString(ClientName.AsSpan(), 260);
            buf.WriteString(MapName.AsSpan(), 260);
            buf.WriteString(GameDirectory.AsSpan(), 260);
            buf.Write(PlaybackTime);
            buf.Write(PlaybackTicks);
            buf.Write(PlaybackFrames);
            buf.Write(SignOnLength);
            return Task.CompletedTask;
        }

        public virtual async Task WriteMessagesAsync(SourceBufferWriter buf)
        {
            foreach (var message in Messages)
            {
                buf.Write(message.Code);
                buf.Write(message.Tick);
                if (message.Slot.HasValue)
                    buf.Write(message.Slot.Value);
                
                await message.Write(buf, this).ConfigureAwait(false);
            }
        }

        public virtual async Task WritePacketsAsync()
        {
            foreach (var message in Messages)
            {
                if (message is Packet pa)
                    await this.Write(pa).ConfigureAwait(false);
            }
        }
        public virtual async Task WriteStringTablesAsync()
        {
            foreach (var message in Messages)
            {
                if (message is StringTables st)
                    await this.Write(st).ConfigureAwait(false);
            }
        }
        public virtual async Task WriteDataTablesAsync()
        {
            foreach (var message in Messages)
            {
                if (message is DataTables dt)
                    await this.Write(dt).ConfigureAwait(false);
            }
        }
        public virtual async Task WriteUserCmdsAsync()
        {
            foreach (var message in Messages)
            {
                if (message is UserCmd uc)
                    await this.Write(uc).ConfigureAwait(false);
            }
        }
    }
}
