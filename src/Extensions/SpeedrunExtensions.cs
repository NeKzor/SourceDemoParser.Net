using System;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Messages;

namespace SourceDemoParser.Extensions
{
    public static class SaveFlags
    {
        public const string SourceRuns = "echo #SAVE#";
    }

    public static class SpeedrunExtensions
    {
        public static Task<SourceDemo> TimeExact(this SourceDemo demo, int endTick = 0, int startTick = 0)
        {
            if (demo.Messages.Count == 0)
                throw new InvalidOperationException("Cannot time demo without any parsed messages.");

            var synced = false;
            var last = 0;
            foreach (DemoMessage msg in demo.Messages)
            {
                if (msg.IsType<SyncTick>())
                    synced = true;

                if (!synced)
                    msg.Tick = 0;
                else if (msg.Tick < 0)
                    msg.Tick = last;

                last = msg.Tick;
            }

            if (endTick < 1)
                endTick = demo.Messages.Last().Tick;

            var delta = endTick - startTick;
            if (delta < 0)
                throw new Exception("Start tick is greater than end tick.");

            var ipt = demo.GetIntervalPerTick();
            demo.PlaybackTicks = delta;
            demo.PlaybackTime = ipt * delta;
            return Task.FromResult(demo);
        }
        public static async Task<SourceDemo> TimeFlagAsync(this SourceDemo demo, string saveFlag = SaveFlags.SourceRuns)
        {
            if (demo.Messages.Count == 0)
                throw new InvalidOperationException("Cannot time demo by flag without any parsed messages.");

            var flag = demo
                .MessagesWhereType(typeof(ConsoleCmd))
                .FirstOrDefault(msg => (msg as ConsoleCmd).Command == saveFlag);

            if (flag != null)
            {
                await demo.TimeExact(flag.Tick).ConfigureAwait(false);
                return demo;
            }

            return demo;
        }
    }
}
