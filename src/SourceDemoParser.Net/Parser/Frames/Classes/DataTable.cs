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
		public string NetTableName;                     // The name matched between client and server
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
		public int Elements { get; set; }				// Number of elements in the array (or 1 if it's not an array)
		public string ExcludeDtName { get; set; }		// If this is an exclude prop, then this is the name of the datatable to exclude a prop from
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
		Unsigned = (1 << 0),                        // Unsigned integer data
		Coord = (1 << 1),                           // If this is set, the float/vector is treated like a world coordinate. Note that the bit count is ignored in this case
		Noscale = (1 << 2),                         // For floating point, don't scale into range, just take value as is
		Rounddown = (1 << 3),                       // For floating point, limit high value to range minus one bit unit
		Roundup = (1 << 4),                         // For floating point, limit low value to range minus one bit unit
		Normal = (1 << 5),                          // If this is set, the vector is treated like a normal (only valid for vectors)
		Exclude = (1 << 6),                         // This is an exclude prop (not excludED, but it points at another prop to be excluded)
		Xyze = (1 << 7),                            // Use XYZ/Exponent encoding for vectors
		InsideArray = (1 << 8),                     // This tells us that the property is inside an array, so it shouldn't be put into the flattened property list. Its array will point at it when it needs to
		ProxyAlwaysYes = (1 << 9),                  // Set for datatable props using one of the default datatable proxies like SendProxy_DataTableToDataTable that always send the data to all clients
		IsAVectorElem = (1 << 10),					// Set automatically if SPROP_VECTORELEM is used
		Collapsible = (1 << 11),                    // Set automatically if it's a datatable with an offset of 0 that doesn't change the pointer
													// (ie: for all automatically-chained base classes)
													// In this case, it can get rid of this SendPropDataTable altogether and spare the
													// trouble of walking the hierarchy more than necessary
		CoordMp = (1 << 12),                        // Like SPROP_COORD, but special handling for multiplayer games
		CoordMpLowPrecision = (1 << 13),            // Like SPROP_COORD, but special handling for multiplayer games where the fractional component only gets a 3 bits instead of 5
		CoordMpIntegral = (1 << 14),                // SPROP_COORD_MP, but coordinates are rounded to integral boundaries
		CellCoord = (1 << 15),                      // SPROP_COORD_MP, but coordinates are rounded to integral boundaries
		CellCoordLowPrecision = (1 << 16),          // SPROP_ENCODED_AGAINST_TICKCOUNT?
		CellCoordIntegral = (1 << 17),				// ?
		ChangesOften = (1 << 18),                   // (1 << 10)? this is an often changed field, moved to head of sendtable so it gets a small index
		VarInt = (1 << 19)							// ?
	}
}