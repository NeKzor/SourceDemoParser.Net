using System;
using System.Collections.Generic;
using System.Reflection;

namespace SourceDemoParser.Net.Extensions
{
	public enum AdjustmentType
	{
		Start,
		End
	}
	
	public enum PlayerStructType
	{
		Position,
		Command
	}
	
	public class AdjustmentCache
	{
		public ISourceDemo Demo { get; set; }
		public string GameDirectory => Demo.GameDirectory;
		public uint DefaultTickrate => Demo.DefaultTickrate;
		public List<Adjustment> Adjustments { get; set; }
	}
	
	public class Adjustment
	{
		public Type Root { get; set; }
		public AdjustmentType Type { get; set; }
		public string MapName { get; set; }
		public int Offset { get; set; }
		public PlayerStructType Parameter { get; set; }
		public MethodInfo Method { get; set; }
		public AdjustmentResult Result { get; set; }
	}
	
	public class AdjustmentResult
	{
		public bool Found { get; set; }
		public int FoundTickAt { get; set; }
	}
}