#if HL2
using System.Collections.Generic;
#endif
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class UserCmd
	{
		public int? CommandNumber { get; set; }
		public int? TickCount { get; set; }
		public QAngle Viewangles { get; set; }
		public float? ForwardMove { get; set; }
		public float? SideMove { get; set; }
		public float? UpMove { get; set; }
		public int? Buttons { get; set; }
		public byte? Impulse { get; set; }
		public int? WeaponSelect { get; set; }
		public int? WeaponSubtype { get; set; }
		//public int RandomSeed { get; set; }
		public short? MouseDx { get; set; }
		public short? MouseDy { get; set; }
		//public bool HasBeenPredicted { get; set; }
#if HL2
		public class CEntityGroundContact
		{
			public int EntIndex { get; set; }
			public float MinHeight { get; set; }
			public float Maxheight { get; set; }
		}
		public List<CEntityGroundContact> EntityGroundContact { get; set; }
#endif
		public const int WEAPON_SUBTYPE_BITS = 6;
	}

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
		Task IFrame.ParseData()
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
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bw = new BitWriter();
			if (Cmd.CommandNumber != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.CommandNumber);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.TickCount != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.TickCount);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.X != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.Y != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Viewangles.Z != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.Viewangles.X).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.ForwardMove != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.ForwardMove).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.SideMove != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.SideMove).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.UpMove != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBytes(((float)Cmd.UpMove).GetBytes());
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Buttons != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt32((int)Cmd.Buttons);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.Impulse != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteByte((byte)Cmd.Impulse);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.WeaponSelect != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteBits((int)Cmd.WeaponSelect, Const.MAX_EDICT_BITS);
				if (Cmd.WeaponSubtype != default)
				{
					bw.WriteOneUBit(1);
					bw.WriteBits((int)Cmd.WeaponSubtype, UserCmd.WEAPON_SUBTYPE_BITS);
				}
				else bw.WriteOneUBit(0);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.MouseDx != default)
			{
				bw.WriteOneUBit(1);
				bw.WriteInt16((short)Cmd.MouseDx);
			}
			else bw.WriteOneUBit(0);
			if (Cmd.MouseDy != default)
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
					bw.WriteBytes(Cmd.EntityGroundContact[i].MinHeight.GetBytes());
					bw.WriteBytes(Cmd.EntityGroundContact[i].Maxheight.GetBytes());
				}
			}
			else bw.WriteOneUBit(0);
#endif

			var bytes = CmdNumber.GetBytes();
			var length = bw.Data.Length.GetBytes();

			length.AppendTo(ref bytes);
			bw.Data.AppendTo(ref bytes);

			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> $"{CmdNumber}";
	}
}