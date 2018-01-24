// TODO: Test hl2 demos
#if HL2
using System.Collections.Generic;
#endif
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
}