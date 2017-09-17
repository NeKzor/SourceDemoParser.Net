using System;
using System.Linq;
using System.Reflection;

namespace SourceDemoParser.Net.Extensions
{
	public class AdjustmentBuilder
	{
		public MethodInfo Method { get; set; }
		
		public AdjustmentBuilder()
		{
		}
		public AdjustmentBuilder(MethodInfo method)
		{
			Method = method;
		}
		
		public Adjustment Build(Type id)
		{
			var adjustment = new Adjustment();
			
			var attribute = Method.GetCustomAttributes().FirstOrDefault(a => a is StartAdjustmentAttribute || a is EndAdjustmentAttribute);
			if (attribute == null)
				throw new Exception("Attribute is null!");
			
			if (attribute.GetType() == typeof(StartAdjustmentAttribute))
			{
				var start = attribute as StartAdjustmentAttribute;
				adjustment.Type = AdjustmentType.Start;
				adjustment.MapName = start.MapName;
				adjustment.Offset = start.Offset;
			}
			else if (attribute.GetType() == typeof(EndAdjustmentAttribute))
			{
				var end = attribute as EndAdjustmentAttribute;
				adjustment.Type = AdjustmentType.End;
				adjustment.MapName = end.MapName;
				adjustment.Offset = end.Offset;
			}
			
			var parameter = Method.GetParameters().FirstOrDefault()?.ParameterType;
			if (parameter == null)
				throw new Exception("Parameter is null!");
			
			if (parameter == typeof(PlayerCommand))
			{
				adjustment.Parameter = PlayerStructType.Command;
			}
			else if (parameter == typeof(PlayerPosition))
			{
				adjustment.Parameter = PlayerStructType.Position;
			}
			else
				throw new Exception("Invalid parameter type!");
			
			adjustment.Root = id;
			adjustment.Method = Method;
			return adjustment;
		}
	}
}