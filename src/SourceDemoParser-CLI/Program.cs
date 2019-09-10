using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using SourceDemoParser;
using SourceDemoParser.Extensions;

namespace SourceDemoParser_CLI
{
    internal class Options
    {
        [Option('p', "parsing-mode", HelpText = "Parsing mode for the parser. 0 = Default, 1 = HeaderOnly, 2 = Everything")]
        public ParsingMode Mode { get; set; }
        [Value(0, HelpText = "Demo file path to parse.", Required = true)]
        public string FilePath { get; set; }
        [Option('o', "output", HelpText = "Output parsed commands for demo info.")]
        public IEnumerable<string> OutputCommands { get; set; }
    }

    internal class Tool
    {
        private SourceDemo _demo;
        private bool _discovered;

        public async Task ParseArgsAsync(Options options)
        {
            try
            {
                if (File.Exists(options.FilePath))
                {
                    _demo = await new SourceParser(ParsingMode.Everything)
                        .ParseFileAsync(options.FilePath);

                    if (_demo != null)
                    {
                        if (options.OutputCommands.Any())
                        {
                            var result = string.Empty;
                            foreach (var command in options.OutputCommands)
                            {
                                var info = await ParseCommandAsync(command);
                                if (info == default) continue;
                                result += $"{info}\n";
                            }
                            if (result != string.Empty)
                                Console.WriteLine("Could not parse any commands!");
                            else
                                Console.Write(result);
                        }
                        else
                        {
                            var command = string.Empty;
                            while (command != "exit")
                            {
                                Console.Write("> ");
                                var result = await ParseCommandAsync(command = Console.ReadLine());
                                if (!string.IsNullOrEmpty(result)) Console.WriteLine(result);
                            }
                        }
                    }
                    else
                        Console.WriteLine("Failed to parse demo.");
                }
                else
                    Console.WriteLine("File does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        public async Task<string> ParseCommandAsync(string command)
        {
            switch (command.ToLower())
            {
                case "header":
                    return $"HeaderId\t{_demo.HeaderId}\n" +
                        $"Protocol\t{_demo.Protocol}\n" +
                        $"NetworkProtocol\t{_demo.NetworkProtocol}\n" +
                        $"GameDirectory\t{_demo.GameDirectory}\n" +
                        $"MapName\t{_demo.MapName}\n" +
                        $"ServerName\t{_demo.ServerName}\n" +
                        $"ClientName\t{_demo.ClientName}\n" +
                        $"PlaybackTime\t{_demo.PlaybackTime.ToString("N3")}\n" +
                        $"PlaybackTicks\t{_demo.PlaybackTicks}\n" +
                        $"PlaybackFrames\t{_demo.PlaybackFrames}\n" +
                        $"SignOnLength\t{_demo.SignOnLength}";
                case "header-id":
                    return $"HeaderId\t{_demo.HeaderId}";
                case "protocol":
                    return $"Protocol\t{_demo.Protocol}";
                case "netproc":
                case "net-protocol":
                    return $"NetworkProtocol\t{_demo.NetworkProtocol}";
                case "dir":
                case "game-dir":
                    return $"GameDirectory\t{_demo.GameDirectory}";
                case "map":
                case "map-name":
                    return $"MapName\t{_demo.MapName}";
                case "server":
                case "server-name":
                    return $"ServerName\t{_demo.ServerName}";
                case "client":
                case "client-name":
                    return $"ClientName\t{_demo.ClientName}";
                case "time":
                    return $"PlaybackTime\t{_demo.PlaybackTime.ToString("N3")}";
                case "ticks":
                    return $"PlaybackTicks\t{_demo.PlaybackTicks}";
                case "frames":
                    return $"FrameCount\t{_demo.PlaybackFrames}";
                case "signon":
                case "signonlength":
                    return $"SignOnLength\t{_demo.SignOnLength}";
                // Data
                case "tickrate":
                    return $"Tickrate\t{_demo.GetTickrate()}";
                case "ipt":
                case "interval-per-tick":
                    return $"IntervalPerTick\t{_demo.GetIntervalPerTick().ToString("N3")}";
                // Returns nothing
                case "adj":
                case "adjust":
                    await _demo.AdjustExact();
                    break;
                case "adj-sf":
                case "adjust-sf":
                    await _demo.AdjustFlagAsync();
                    break;
                case "adj2":
                case "adjust2":
                    if (!_discovered)
                    {
                        await Adjustments.DiscoverAsync();
                        _discovered = true;
                    }
                    await _demo.AdjustAsync();
                    break;
            }
            return default;
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var result = Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(options => new Tool()
                    .ParseArgsAsync(options)
                    .GetAwaiter()
                    .GetResult());
        }
    }
}
