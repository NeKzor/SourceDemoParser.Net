using System;

namespace SourceDemoParser.Extensions
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EndAdjustmentAttribute : Attribute
	{
		public string MapName { get; }
		public int Offset { get; }

		public EndAdjustmentAttribute(string map = default, int offset = default)
		{
			MapName = map;
			Offset = offset;
		}
	}
}