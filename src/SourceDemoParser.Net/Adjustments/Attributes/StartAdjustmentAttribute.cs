using System;

namespace SourceDemoParser.Extensions
{
	[AttributeUsage(AttributeTargets.Method)]
	public class StartAdjustmentAttribute : Attribute
	{
		public string MapName { get; }
		public int Offset { get; }

		public StartAdjustmentAttribute(string map = null, int offset = 0)
		{
			MapName = map;
			Offset = offset;
		}
	}
}