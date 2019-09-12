using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Engine;
using SourceDemoParser.Messages;

namespace SourceDemoParser.Extensions
{
    public static class ParserExtensions
    {
        public static Task Read(this SourceDemo demo, StringTables frame)
        {
            var buf = frame.Buffer;
            int tables = buf.ReadByte();
            for (int i = 0; i < tables; i++)
            {
                var name = buf.ReadString();
                var table = new StringTable(name);

                var entries = buf.ReadInt16();
                for (int j = 0; j < entries; j++)
                {
                    var entry = buf.ReadString();
                    var data = default(byte[]);
                    var version = default(long);
                    var xuid = default(long);
                    var info = default(TableInfoBase);

                    if (buf.ReadBoolean())
                    {
                        var length = buf.ReadInt16();
                        data = buf.ReadBytes(length);

                        // TODO
                        if (name == Const.INSTANCE_BASELINE_TABLENAME)
                        {
                            info = new InstanceBaseline()
                            {
                                Id = int.Parse(entry)
                            };
                        }
                        else if (name == Const.LIGHT_STYLES_TABLENAME)
                        {

                        }
                        else if (name == Const.SERVER_STARTUP_DATA_TABLENAME)
                        {

                        }
                        else if (name == Const.USER_INFO_TABLENAME)
                        {
                            var buf2 = new SourceBufferReader(data);
                            if (demo.GameDirectory == "csgo")
                            {
                                // 8 bytes
                                version = BitConverter.ToInt64(buf2.ReadBytes(8), 0);
                                // 8 bytes
                                xuid = BitConverter.ToInt64(buf2.ReadBytes(8), 0);
                            }
                            else
                            {
                                // 4 bytes
                                version = buf2.ReadInt32();
                                // 4 bytes
                                xuid = buf2.ReadInt32();
                            }

                            info = new PlayerInfo()
                            {
                                // 32 bytes
                                Name = Encoding.ASCII.GetString(buf2.ReadBytes(Const.MAX_PLAYER_NAME_LENGTH)).TrimEnd('\0'),
                                // 4 bytes
                                UserId = buf2.ReadInt32(),
                                // 33 bytes
                                Guid = Encoding.ASCII.GetString(buf2.ReadBytes(Const.SIGNED_GUID_LEN + 1)).TrimEnd('\0'),
                                // 4 bytes
                                FriendsId = buf2.ReadInt32(),
                                // 32 bytes
                                FriendsName = Encoding.ASCII.GetString(buf2.ReadBytes(Const.MAX_PLAYER_NAME_LENGTH)).TrimEnd('\0'),
                                // 1 byte
                                Fakeplayer = buf2.ReadBoolean(),
                                // 1 byte
                                IsHltv = buf2.ReadBoolean(),
                                // 16 bytes
                                CustomFiles = new int[4]
                                {
                                    buf2.ReadInt32(),
                                    buf2.ReadInt32(),
                                    buf2.ReadInt32(),
                                    buf2.ReadInt32()
                                },
                                // 2 bytes
                                FilesDownloaded = buf2.ReadChar()
                            };
                        }
                    }
                    table.AddEntry(new StringTableEntry()
                    {
                        Name = entry,
                        RawData = data,
                        Version = version,
                        Xuid = xuid,
                        Info = info
                    });
                }

                if (buf.ReadBoolean())
                {
                    var centries = buf.ReadInt16();
                    for (var j = 0; j < centries; j++)
                    {
                        var centry = buf.ReadString();
                        var ddata = default(byte[]);
                        if (buf.ReadBoolean())
                        {
                            var length = buf.ReadInt16();
                            ddata = buf.ReadBytes(length);
                        }
                        table.AddClientEntry(new ClientEntry()
                        {
                            Name = centry,
                            RawData = ddata
                        });
                    }
                }
                frame.Tables.Add(table);
            }
            return Task.CompletedTask;
        }
        public static Task Read(this SourceDemo demo, DataTables frame)
        {
            var buf = frame.Buffer;
            while (buf.ReadBoolean())
            {
                bool needsdecoder = buf.ReadBoolean();
                var table = new SendTable()
                {
                    NetTableName = buf.ReadString(),
                    NeedsDecoder = needsdecoder
                };

                var props = buf.ReadUBits(DataTable.PROPINFOBITS_NUMPROPS);
                for (int j = 0; j < props; j++)
                {
                    int fbits = (demo.Protocol == 2)
                        ? 11
                        : DataTable.PROPINFOBITS_FLAGS;

                    var prop = new SendProp
                    {
                        Type = (SendPropType)buf.ReadUBits(DataTable.PROPINFOBITS_TYPE),
                        VarName = buf.ReadString(),
                        Flags = (SendPropFlags)buf.ReadUBits(fbits)
                    };

                    if ((prop.Type == SendPropType.DataTable) || (prop.IsExcludeProp()))
                    {
                        prop.ExcludeDtName = buf.ReadString();
                        //if ((prop.Flags & SendPropFlags.Collapsible) != 0)
                        //{
                        //}
                    }
                    else if (prop.Type == SendPropType.Array)
                    {
                        prop.Elements = (int)buf.ReadUBits(DataTable.PROPINFOBITS_NUMELEMENTS);
                    }
                    else
                    {
                        prop.LowValue = buf.ReadSingle();
                        prop.HighValue = buf.ReadSingle();
                        prop.Bits = (int)buf.ReadUBits(DataTable.PROPINFOBITS_NUMBITS + 1);
                    }
                    table.Props.Add(prop);
                }

                frame.Tables.Add(table);
            }

            var classes = buf.ReadInt16();
            for (var i = 0; i < classes; i++)
            {
                frame.Classes.Add(new ServerClassInfo
                {
                    ClassId = buf.ReadInt16(),
                    ClassName = buf.ReadString(),
                    DataTableName = buf.ReadString()
                });
            }
            return Task.CompletedTask;
        }
        public static async Task Read(this SourceDemo demo, Packet frame)
        {
            var buf = frame.Buffer;
            while (buf.BitsLeft > 6)
            {
                var code = (byte)buf.ReadUBits(6);
                var type = demo.Game.DefaultNetMessages.ElementAtOrDefault(code);
                if (type == null)
                    throw new Exception($"Unknown net message {code} at {buf.CurrentByte}.");

                var message = (NetMessage)Activator.CreateInstance(type);

                message.Code = code;

                await message.Read(buf, demo).ConfigureAwait(false);
                frame.NetMessages.Add(message);
            }
        }
        public static Task Read(this SourceDemo demo, UserCmd frame)
        {
            var buf = frame.Buffer;
            frame.Cmd.CommandNumber = buf.ReadField<int>();
            frame.Cmd.TickCount = buf.ReadField<int>();
            frame.Cmd.ViewanglesX = buf.ReadField<float>();
            frame.Cmd.ViewanglesY = buf.ReadField<float>();
            frame.Cmd.ViewanglesZ = buf.ReadField<float>();
            frame.Cmd.ForwardMove = buf.ReadField<float>();
            frame.Cmd.SideMove = buf.ReadField<float>();
            frame.Cmd.UpMove = buf.ReadField<float>();
            frame.Cmd.Buttons = buf.ReadField<int>();
            frame.Cmd.Impulse = buf.ReadField<byte>();
            frame.Cmd.Buttons = buf.ReadField<int>();
            frame.Cmd.WeaponSelect = buf.ReadBitField<int>(Const.MAX_EDICT_BITS, () =>
            {
                frame.Cmd.WeaponSubtype = buf.ReadBitField<int>(UserCmdInfo.WEAPON_SUBTYPE_BITS);
            });
            frame.Cmd.MouseDx = buf.ReadField<short>();
            frame.Cmd.MouseDy = buf.ReadField<short>();
            return Task.CompletedTask;
        }
    }
}
