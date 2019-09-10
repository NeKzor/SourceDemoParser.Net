using System;
using System.Reflection;

namespace SourceDemoParser.Extensions
{
    internal class Adjustment
    {
        public TypeInfo Root { get; set; }
        public AdjustmentType Type { get; set; }
        public string MapName { get; set; }
        public int Offset { get; set; }
        public PlayerStructType Parameter { get; set; }
        public MethodInfo Method { get; set; }
        public AdjustmentResult Result { get; set; }
    }
}
