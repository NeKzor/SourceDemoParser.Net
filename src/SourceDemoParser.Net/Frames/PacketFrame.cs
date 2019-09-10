using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
    public class PacketFrame : IDemoFrame
    {
        public byte[] Data { get; set; }
        public List<PacketInfo> Infos { get; set; }
        public int InSequence { get; set; }
        public int OutSequence { get; set; }
        public List<INetMessage> NetMessages { get; set; }

        public PacketFrame()
        {
            Infos = new List<PacketInfo>();
            NetMessages = new List<INetMessage>();
        }

        Task IDemoFrame.Parse(SourceDemo demo)
        {
            var buf = new SourceBufferReader(Data);
            for (int i = 0; i < (demo.Game.MaxSplitscreenClients ?? 1); i++)
            {
                // 76 bytes
                Infos.Add(new PacketInfo
                {
                    Flags = (DemoFlags)buf.ReadInt32(),
                    ViewOrigin = new Vector(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
                    ViewAngles = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
                    LocalViewAngles = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
                    ViewOrigin2 = new Vector(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
                    ViewAngles2 = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle()),
                    LocalViewAngles2 = new QAngle(buf.ReadSingle(), buf.ReadSingle(), buf.ReadSingle())
                });
            }
            InSequence = buf.ReadInt32();
            OutSequence = buf.ReadInt32();

            while (buf.BitsLeft > 6)
            {
                var code = (int)buf.ReadUBits(6);
                var type = demo.Game.DefaultNetMessages.ElementAtOrDefault(code);
                if (type == null)
                {
                    throw new Exception($"Unknown net message {code} at {buf.CurrentByte}.");
                }

                var message = type.GetMessage();
                _ = message.Parse(buf, demo).ConfigureAwait(false);
                NetMessages.Add(message);
            }

            return Task.CompletedTask;
        }
        Task<byte[]> IDemoFrame.Export(SourceDemo demo)
        {
            var data = new byte[0];
            foreach (var info in Infos)
            {
                ((int)info.Flags).ToBytes().AppendTo(ref data);
                info.ViewOrigin.ToBytes().AppendTo(ref data);
                info.ViewAngles.ToBytes().AppendTo(ref data);
                info.LocalViewAngles.ToBytes().AppendTo(ref data);
                info.ViewOrigin2.ToBytes().AppendTo(ref data);
                info.ViewAngles2.ToBytes().AppendTo(ref data);
                info.LocalViewAngles2.ToBytes().AppendTo(ref data);
            }
            InSequence.ToBytes().AppendTo(ref data);
            OutSequence.ToBytes().AppendTo(ref data);
            var bw = new SourceBufferWriter(data);
            foreach (var msg in NetMessages)
                _ = msg.Export(bw, demo).ConfigureAwait(false);
            return Task.FromResult(bw.Data);
        }
    }
}