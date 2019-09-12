using System;
using System.Threading.Tasks;
using SourceDemoParser.Messages;

namespace SourceDemoParser.Extensions
{
    public static class ExporterExtensions
    {
        public static Task Write(this SourceDemo demo, StringTables frame)
        {
            throw new NotImplementedException();
        }
        public static Task Write(this SourceDemo demo, DataTables frame)
        {
            throw new NotImplementedException();
        }
        public static Task Write(this SourceDemo demo, Packet frame)
        {
            throw new NotImplementedException();
        }
        public static Task Write(this SourceDemo demo, UserCmd frame)
        {
            throw new NotImplementedException();
            /* frame.Buffer.WriteField(Cmd.CommandNumber);
            frame.Buffer.WriteField(Cmd.TickCount);
            frame.Buffer.WriteField(Cmd.ViewanglesX);
            frame.Buffer.WriteField(Cmd.ViewanglesY);
            frame.Buffer.WriteField(Cmd.ViewanglesZ);
            frame.Buffer.WriteField(Cmd.ForwardMove);
            frame.Buffer.WriteField(Cmd.SideMove);
            frame.Buffer.WriteField(Cmd.Buttons);
            frame.Buffer.WriteBitField(Cmd.WeaponSelect, Const.MAX_EDICT_BITS, () =>
            {
                frame.Buffer.WriteBitField(Cmd.WeaponSubtype, UserCmdInfo.WEAPON_SUBTYPE_BITS);
            });
            frame.Buffer.WriteField(Cmd.MouseDx);
            frame.Buffer.WriteField(Cmd.MouseDy);

            frame.Buffer.Write(CmdNumber);
            frame.Buffer.WriteBufferield(Buffer.Data); */
        }
    }
}
