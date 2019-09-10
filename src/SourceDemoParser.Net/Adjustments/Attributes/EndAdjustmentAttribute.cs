using System;

namespace SourceDemoParser.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndAdjustmentAttribute : Attribute
    {
        public string MapName { get; }
        public int Offset { get; }

        public EndAdjustmentAttribute(string map = null, int offset = 0)
        {
            MapName = map;
            Offset = offset;
        }
    }
}
