using System;

namespace SourceDemoParser.Extensions
{
	[AttributeUsage(AttributeTargets.Method)]
	public class StartAdjustmentAttribute : Attribute
	{
		public string MapName { get; }
		public int Offset { get; }

		public StartAdjustmentAttribute(string map = default, int offset = default)
		{
			MapName = map;
			Offset = offset;
		}
	}
}