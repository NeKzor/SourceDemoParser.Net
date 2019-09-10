using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SourceDemoParser.Tests
{
    internal class Splicer
    {
        public async Task Splice()
        {
            const string source1 = "ademo.dem";
            const string source2 = "ademo2.dem";
            const string source3 = "ademo2.dem";
            const string output = "splice.dem";

            var demos = new List<SourceDemo>();
            var files = new List<string>()
            {
                Paths.Test + source1,
                Paths.Test + source2,
                Paths.Test + source3,
            };

            var parser = new SourceParser(ParsingMode.Default, AdjustmentType.Exact);
            var exporter = new SourceExporter(ExportMode.Default);

            if (files.Count < 2)
                throw new InvalidOperationException("Need at least two demos for merging them together.");

            foreach (var source in files)
            {
                var demo = await parser.ParseFileAsync(source);
                demo.Messages.RemoveAll(m => m.Type.Name == "Stop");
                demos.Add(demo);
            }

            var result = demos.First();
            var tick = 0;
            foreach (var demo in demos.Skip(1))
            {
                tick = result.PlaybackTicks;

                result.PlaybackTicks += demo.PlaybackTicks;
                result.PlaybackTime += demo.PlaybackTime;
                result.PlaybackFrames += demo.PlaybackFrames;

                var messages = demo.Messages.Where(m =>
                {
                    return (m.Frame is PacketFrame)
                        || (m.Frame is DataTablesFrame)
                        || (m.Frame is UserCmdFrame)
                        || (m.Frame is ConsoleCmdFrame)
                        || (m.Frame is StopFrame);
                });

                foreach (var msg in messages)
                    msg.Tick += tick;

                result.Messages.AddRange(messages);
            }
            result.Messages.Add(new Types.Stop(0x07).GetMessage());

            await exporter.ExportFileAsync(result, Paths.Export + output);
        }
        public async Task Analyze()
        {
            const string source = "splice.dem";
            var parser = new SourceParser(ParsingMode.Everything, AdjustmentType.Exact);
            var demo = await parser.ParseFileAsync(Paths.Export + source);

            using (var fs = File.Create(Paths.Logs + "test.txt"))
            using (var sr = new StreamWriter(fs))
            {
                foreach (var msg in demo.Messages)
                {
                    sr.WriteLine(msg);
                }
            }
        }
    }
}
