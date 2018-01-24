using System;
using System.Linq;
using System.Reflection;

namespace SourceDemoParser.Extensions
{
	internal class AdjustmentBuilder
	{
		public MethodInfo Method { get; set; }

		public AdjustmentBuilder()
		{
		}
		public AdjustmentBuilder(MethodInfo method)
		{
			Method = method;
		}

		public Adjustment Build(TypeInfo id)
		{
			var attribute = Method
				.GetCustomAttributes()
				.FirstOrDefault(a => (a is StartAdjustmentAttribute) || (a is EndAdjustmentAttribute));
			if (attribute == null)
				throw new Exception("Attribute is null!");

			var adjustment = default(Adjustment);
			if (attribute is StartAdjustmentAttribute start)
			{
				adjustment = new Adjustment
				{
					Type = AdjustmentType.Start,
					MapName = start.MapName,
					Offset = start.Offset
				};
			}
			else if (attribute is EndAdjustmentAttribute end)
			{
				adjustment = new Adjustment
				{
					Type = AdjustmentType.End,
					MapName = end.MapName,
					Offset = end.Offset
				};
			}

			var parameter = Method
				.GetParameters()
				.FirstOrDefault()?.ParameterType;
			if (parameter == null)
				throw new Exception("Parameter is null!");

			if (parameter == typeof(PlayerCommand))
				adjustment.Parameter = PlayerStructType.Command;
			else if (parameter == typeof(PlayerPosition))
				adjustment.Parameter = PlayerStructType.Position;
			else
				throw new Exception("Invalid parameter type!");

			adjustment.Root = id;
			adjustment.Method = Method;
			return adjustment;
		}
	}
}