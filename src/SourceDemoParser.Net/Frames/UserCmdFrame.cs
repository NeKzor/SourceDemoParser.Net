#if HL2
using System.Collections.Generic;
#endif
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class UserCmdFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public int CmdNumber { get; set; }
		public UserCmd Cmd { get; set; }

		public UserCmdFrame()
		{
		}
		public UserCmdFrame(int cmd, byte[] data)
		{
			CmdNumber = cmd;
			RawData = data;
		}
		Task IFrame.ParseData(SourceDemo demo)
		{
			var buf = new BitBuffer(RawData);
			Cmd = new UserCmd();
			if (buf.ReadBoolean()) Cmd.CommandNumber = buf.ReadBits(32);
			if (buf.ReadBoolean()) Cmd.TickCount = buf.ReadBits(32);
			Cmd.Viewangles = new QAngle();
			if (buf.ReadBoolean()) Cmd.Viewangles.X = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.Viewangles.Y = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.Viewangles.Z = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.ForwardMove = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.SideMove = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.UpMove = buf.ReadSingle();
			if (buf.ReadBoolean()) Cmd.Buttons = buf.ReadBits(32);
			if (buf.ReadBoolean()) Cmd.Impulse = buf.ReadByte();
			if (buf.ReadBoolean())
			{
				Cmd.WeaponSelect = buf.ReadBits(Const.MAX_EDICT_BITS);
				if (buf.ReadBoolean()) Cmd.WeaponSubtype = buf.ReadBits(UserCmd.WEAPON_SUBTYPE_BITS);
			}
			if (buf.ReadBoolean()) Cmd.MouseDx = buf.ReadInt16();
			if (buf.ReadBoolean()) Cmd.MouseDy = buf.ReadInt16();
#if HL2
			if (buf.ReadBoolean())
			{
				var count = buf.ReadInt16();
				Cmd.EntityGroundContact = new List<UserCmd.CEntityGroundContact>();
				for (int i = 0; i < count; i++)
				{
					var entity = new UserCmd.CEntityGroundContact
					{
						EntIndex = buf.ReadBits(UserCmd.MAX_EDICT_BITS),
						MinHeight = buf.ReadSingle(),
						Maxheight = buf.ReadSingle()
					};
					Cmd.EntityGroundContact.Add(entity);
				}
			}
#endif
			return Task.CompletedTask;
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			CmdNumber.ToBytes().AppendTo(ref data);
			var bw = new BitWriter();
			if (Cmd.CommandNumber != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.CommandNumber);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.TickCount != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.TickCount);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.X != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.Y != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.Z != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.ForwardMove != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.ForwardMove).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.SideMove != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.SideMove).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.UpMove != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.UpMove).ToBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Buttons != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.Buttons);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Impulse != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteByte((byte)Cmd.Impulse);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.WeaponSelect != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteBits((int)Cmd.WeaponSelect, Const.MAX_EDICT_BITS);
				if (Cmd.WeaponSubtype != null)
				{
					bw.WriteOneUBit(1);
					bw.WriteBits((int)Cmd.WeaponSubtype, UserCmd.WEAPON_SUBTYPE_BITS);
				}
				else bw.WriteOneUBit(0);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.MouseDx != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt16((short)Cmd.MouseDx);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.MouseDy != null)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt16((short)Cmd.MouseDy);
			}
			else bw.WriteOneUBit(0);
#if HL2
			if ((Cmd.EntityGroundContact?.Count ?? 0) != 0)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32(Cmd.EntityGroundContact.Count);
				for (int i = 0; i < Cmd.EntityGroundContact.Count; i++)
				{
					bw.WriteBits(Cmd.EntityGroundContact[i].EntIndex, UserCmd.MAX_EDICT_BITS);
					bw.WriteBytes(Cmd.EntityGroundContact[i].MinHeight.ToBytes());
					bw.WriteBytes(Cmd.EntityGroundContact[i].Maxheight.ToBytes());
				}
			}
			else bw.WriteOneUBit(0);
#endif
			bw.Data.ToBytes().AppendTo(ref data);
			return Task.FromResult(data);
		}
	}
}