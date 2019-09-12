using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SourceDemoParser.Messages;
using SourceDemoParser.Speedrun;

namespace SourceDemoParser.Tests
{
    internal class Test
    {
        public class CustomDemo : ISourceDemo
        {
            public string GameDirectory => "portal2";
            public uint DefaultTickrate => 60u;

            [StartAdjustment]
            public bool Custom_Start(PlayerCommand cmd)
            {
                return cmd.Current.StartsWith("voice_modenable");
            }
        }

        public async Task Parsing()
        {
            const string source = "portal2_sp.dem";

            var parser = new SourceParser();
            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
        }
        // TODO: Real benchmarking
        public async Task Timing()
        {
            const string source = "portal2_sp.dem";

            var watch = new Stopwatch();

            var fast = new SourceParser();
            var slow = new SourceParser(new SourceParserOptions()
            {
                ReadPackets = true,
                ReadDataTables = true,
                ReadStringTables = true,
                ReadUserCmds = true,
            });
            var headeronly = new SourceParser(new SourceParserOptions()
            {
                ReadMessages = false
            });

            // Everything
            watch = Stopwatch.StartNew();
            _ = await slow.ParseFromFileAsync(Paths.Demos + source);
            watch.Stop();
            var result2 = watch.Elapsed.TotalMilliseconds;

            // Default
            watch = Stopwatch.StartNew();
            _ = await fast.ParseFromFileAsync(Paths.Demos + source);
            watch.Stop();
            var result1 = watch.Elapsed.TotalMilliseconds;

            // Header only
            watch = Stopwatch.StartNew();
            _ = await headeronly.ParseFromFileAsync(Paths.Demos + source);
            watch.Stop();
            var result3 = watch.Elapsed.TotalMilliseconds;

            Console.WriteLine($"Default: {result1}ms");
            Console.WriteLine($"Everything: {result2}ms ({(int)(result2 / result1)} times slower)");
            Console.WriteLine($"Header only: {result3}ms ({(int)(result1 / result3)} times faster)");

            /*	Random Result
				Default: 33.0111ms
				Everything: 82.6472ms (2 times slower)
				Header only: 0.1249ms (264 times faster)
			*/
        }
        public async Task Modification()
        {
            const string source = "portal2_sp.dem";
            const string destination = "portal2_sp_edit.dem";

            var parser = new SourceParser();
            var exporter = new SourceExporter();

            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
            var demo2 = default(SourceDemo);

            await exporter.ExportToFileAsync(Paths.Export + destination, demo);
            try
            {
                Console.WriteLine("Parsing exported demo...");
                demo2 = await parser.ParseFromFileAsync(Paths.Export + destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public async Task Discover()
        {
            try
            {
                const string source = "portal2_cm_coop.dem";

                var parser = new SourceParser();
                var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
                Console.WriteLine("Before: " + demo.PlaybackTicks);

                // Load automatically from assembly
#if !DISCOVER_2
                await Adjustments.DiscoverAsync();
#else
				await Adjustments.DiscoverAsync(System.Reflection.Assembly.GetEntryAssembly());
#endif
                await demo.AdjustAsync();
                Console.WriteLine("After: " + demo.PlaybackTicks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public async Task DirectLoad()
        {
            try
            {
                const string source = "portal2_cm_coop.dem";

                var parser = new SourceParser();
                var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
                Console.WriteLine("Before: " + demo.PlaybackTicks);

                // Load custom demo directly
                await Adjustments.LoadAsync<CustomDemo>();
                await demo.AdjustAsync();
                Console.WriteLine("After: " + demo.PlaybackTicks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public async Task Cleanup()
        {
            const string source = "portal2_pausing.dem";

            var parser = new SourceParser();
            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);

            bool RemovePacket = false;
            var copy = demo.Messages.ToArray();
            demo.Messages.CopyTo(copy);

            var index = 0;
            foreach (var msg in copy)
            {
                if (msg.Tick > 0)
                {
                    if (msg is ConsoleCmd console)
                    {
                        if (console.Command == "gameui_activate")
                            RemovePacket = true;
                        else if (console.Command == "gameui_hide")
                            RemovePacket = false;
                    }
                    else if ((msg is UserCmd) || (msg is Packet))
                    {
                        if (RemovePacket)
                        {
                            demo.Messages.RemoveAt(index);
                            --index;
                        }
                    }
                }
                ++index;
            }

            var exporter = new SourceExporter();
            await exporter.ExportToFileAsync(Paths.Demos + "portal2_cleanup.dem", demo);
        }
        public async Task Net()
        {
            const string source = "portal2.dem";

            var parser = new SourceParser(new SourceParserOptions()
            {
                ReadPackets = true
            });

            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);

            var dump = new System.Collections.Generic.List<string>();
            foreach (var msg in demo.Messages)
            {
                if (msg is Packet packet)
                {
                    dump.Add($"--- tick {msg.Tick} ---");
                    foreach (var nmsg in packet.NetMessages)
                    {
                        dump.Add($"{nmsg.Code} {nmsg.Name}");
                    }
                }
            }

            await System.IO.File.WriteAllLinesAsync("logs/dump.txt", dump);
        }
    }
}
