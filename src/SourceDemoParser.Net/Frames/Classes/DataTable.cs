using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SourceDemoParser
{
	public static class DataTable
	{
		// dt_common.h
		public const int SPROP_NUMFLAGBITS_NETWORKED = 16;

		// dt.h
		public const int PROPINFOBITS_NUMPROPS = 10;
		public const int PROPINFOBITS_TYPE = 5;
		public const int PROPINFOBITS_FLAGS = SPROP_NUMFLAGBITS_NETWORKED;
		public const int PROPINFOBITS_NUMELEMENTS = 10;
		public const int PROPINFOBITS_NUMBITS = 6;
		//public const int PROPINFOBITS_STRINGBUFFERLEN = 10;
		//public const int PROPINFOBITS_RIGHTSHIFT = 6;
	}

	[DebuggerDisplay("{NetTableName,nq} ({Count,nq})")]
	public class SendTable
	{
		public List<SendProp> Props { get; set; }
		public int Count => Props.Count;
		public string NetTableName;
		public bool NeedsDecoder { get; set; }

		public SendTable()
		{
			Props = new List<SendProp>();
		}

		public override string ToString()
		{
			return $"{NetTableName}{((NeedsDecoder) ? "*" : string.Empty)} ({Props.Count})";
		}
	}

	[DebuggerDisplay("[{(int)Flags,nq}] DPT_{Type,nq} ({VarName})")]
	public class SendProp
	{
		public SendPropType Type { get; set; }
		public int Bits { get; set; }
		public float LowValue { get; set; }
		public float HighValue { get; set; }
		public int Elements { get; set; }
		public string ExcludeDtName { get; set; }
		public string VarName { get; set; }
		public SendPropFlags Flags { get; set; }

		public bool IsExcludeProp()
		{
			return (Flags & SendPropFlags.Exclude) != 0;
		}
	}

	[DebuggerDisplay("[{ClassId,nq}] {ClassName,nq} ({DataTableName,nq})")]
	public class ServerClassInfo
	{
		public string ClassName { get; set; }
		public string DataTableName { get; set; }
		public int ClassId { get; set; }
	}

	// dt_common.h
	public enum SendPropType
	{
		Int = 0,
		Float,
		Vector,
		VectorXy,		// ?
		String,
		Array,
		DataTable,
		Int64			//?
	}
	[Flags]
	public enum SendPropFlags
	{
		Unsigned = (1 << 0),
		Coord = (1 << 1),
		Noscale = (1 << 2),
		Rounddown = (1 << 3),
		Roundup = (1 << 4),
		Normal = (1 << 5),
		Exclude = (1 << 6),
		Xyze = (1 << 7),
		InsideArray = (1 << 8),
		ProxyAlwaysYes = (1 << 9),
		IsAVectorElem = (1 << 10),
		Collapsible = (1 << 11),                    
		CoordMp = (1 << 12),
		CoordMpLowPrecision = (1 << 13),
		CoordMpIntegral = (1 << 14),
		CellCoord = (1 << 15),
		CellCoordLowPrecision = (1 << 16),
		CellCoordIntegral = (1 << 17),
		ChangesOften = (1 << 18),
		VarInt = (1 << 19)
	}
}