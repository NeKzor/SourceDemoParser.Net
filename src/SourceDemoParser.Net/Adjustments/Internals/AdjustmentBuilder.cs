using System;
using System.Linq;
using System.Reflection;

namespace SourceDemoParser.Extensions
{
    internal class AdjustmentBuilder
    {
        public TypeInfo Id { get; set; }
        public MethodInfo Method { get; set; }

        public AdjustmentBuilder()
        {
        }
        public AdjustmentBuilder(TypeInfo id, MethodInfo method)
        {
            Id = id;
            Method = method;
        }

        public AdjustmentBuilder WithId(TypeInfo id)
        {
            Id = id;
            return this;
        }
        public AdjustmentBuilder WithMethod(MethodInfo method)
        {
            Method = method;
            return this;
        }

        public Adjustment Build()
        {
            var attribute = Method
                .GetCustomAttributes()
                .FirstOrDefault(a => (a is StartAdjustmentAttribute) || (a is EndAdjustmentAttribute));
            if (attribute == null)
                throw new Exception("Start or end attribute not found.");

            var adjustment = default(Adjustment);
            if (attribute is StartAdjustmentAttribute start)
            {
                adjustment = new Adjustment()
                {
                    Type = AdjustmentType.Start,
                    MapName = start.MapName,
                    Offset = start.Offset
                };
            }
            else if (attribute is EndAdjustmentAttribute end)
            {
                adjustment = new Adjustment()
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
                throw new Exception("Method doesn't have any parameters.");

            if (parameter == typeof(PlayerCommand))
                adjustment.Parameter = PlayerStructType.Command;
            else if (parameter == typeof(PlayerPosition))
                adjustment.Parameter = PlayerStructType.Position;
            else
                throw new Exception("Type of parameter is not supported.");

            adjustment.Root = Id;
            adjustment.Method = Method;
            return adjustment;
        }
    }
}
