using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceDemoParser.Extensions
{
    public static class DemoExtensions
    {
        // Header
        public static int GetTickrate(this SourceDemo demo)
            => (int)Math.Round(demo.PlaybackTicks / demo.PlaybackTime);
        public static float GetIntervalPerTick(this SourceDemo demo)
            => demo.PlaybackTime / demo.PlaybackTicks;

        // Data
        public static IEnumerable<IDemoMessage> MessagesWhereType(this SourceDemo demo, Type type)
            => demo.Messages
                .Where(message => message.GetType() == type);
        public static IEnumerable<IDemoMessage> MessagesWhereType(this SourceDemo demo, string name)
            => demo.Messages
                .Where(message => message.Name == name);
        public static IEnumerable<IDemoMessage> MessagesWhereType(this SourceDemo demo, byte code)
            => demo.Messages
                .Where(message => message.Code == code);
        public static IEnumerable<IDemoMessage> MessagesWhereTick(this SourceDemo demo, int tick)
            => demo.Messages
                .Where(message => message.Tick == tick);
    }
}
