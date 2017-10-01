using System;

namespace SourceDemoParser.Extensions
{
	[AttributeUsage(AttributeTargets.Method)]
	public class StartAdjustmentAttribute : Attribute
	{
		public string MapName { get; }
		public int Offset { get; }

		public StartAdjustmentAttribute(string map = default(string), int offset = default(int))
		{
			MapName = map;
			Offset = offset;
		}
	}
}