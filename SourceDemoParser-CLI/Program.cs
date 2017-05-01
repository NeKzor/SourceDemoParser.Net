using System;
using System.Linq;
using SourceDemoParser.Net;

namespace SourceDemoParser_CLI
{
	internal static class Program
	{
		private static SourceDemo _demo;

		private static void Main(string[] args)
		{
			if ((args != null) && (args?.Length >= 2))
			{
				if (SourceDemo.TryParse(string.Join(" ", args.Skip(1)), out _demo).GetAwaiter().GetResult())
				{
					var output = string.Empty;
					foreach (var command in args[0].Split(';'))
					{
						var temp = ParseCommand(command);
						output += (temp != default(string)) ? $"{temp}\n" : string.Empty;
					}
					Console.Write((output != string.Empty) ? output.Substring(0, output.Length - 1) : "Could not parse any commands!");
				}
				else
				{
					Console.Write("Could not parse the demo file!");
				}
			}
		}

		private static string ParseCommand(string command)
		{
			switch (command)
			{
				case "parse":
					return $"GameInfo\t{_demo.GameInfo}\n" +
						$"DemoProtocol\t{_demo.DemoProtocol}\n" +
						$"NetworkProtocol\t{_demo.NetworkProtocol}\n" +
						$"FilePath\t{_demo.FilePath}\n" +
						$"GameDirectory\t{_demo.GameDirectory}\n" +
						$"MapName\t{_demo.MapName}\n" +
						$"Server\t{_demo.Server}\n" +
						$"Client\t{_demo.Client}\n" +
						$"PlaybackTime\t{_demo.PlaybackTime.ToString("N3")}\n" +
						$"PlaybackTicks\t{_demo.PlaybackTicks}\n" +
						$"FrameCount\t{_demo.FrameCount}\n" +
						$"SignOnLength\t{_demo.SignOnLength}\n" +
						$"StartAdjustmentTick\t{_demo.StartAdjustmentTick}\n" +
						$"EndAdjustmentTick\t{_demo.EndAdjustmentTick}\n" +
						$"StartAdjustmentType\t{_demo.StartAdjustmentType}\n" +
						$"EndAdjustmentType\t{_demo.EndAdjustmentType}\n" +
						$"AdjustedTime\t{_demo.AdjustedTime.ToString("N3")}\n" +
						$"AdjustedTicks\t{_demo.AdjustedTicks}\n" +
						$"Tickrate\t{_demo.Tickrate}\n" +
						$"TicksPerSecond\t{_demo.TicksPerSecond.ToString("N3")}";
				case "version":
					return $"DemoProtocol\t{_demo.DemoProtocol}";
				case "netproc":
					return $"NetworkProtocol\t{_demo.NetworkProtocol}";
				case "path":
					return $"FilePath\t{_demo.FilePath}";
				case "dir":
					return $"GameDirectory\t{_demo.GameDirectory}";
				case "map":
					return $"MapName\t{_demo.MapName}";
				case "server":
					return $"Server\t{_demo.Server}";
				case "client":
					return $"Client\t{_demo.Client}";
				case "time":
					return $"PlaybackTime\t{_demo.PlaybackTime.ToString("N3")}";
				case "ticks":
					return $"PlaybackTicks\t{_demo.PlaybackTicks}";
				case "frames":
					return $"FrameCount\t{_demo.FrameCount}";
				case "signon":
					return $"SignOnLength\t{_demo.SignOnLength}";
				case "tickstart":
					return $"StartAdjustmentTick\t{_demo.StartAdjustmentTick}";
				case "tickend":
					return $"EndAdjustmentTick\t{_demo.EndAdjustmentTick}";
				case "starttype":
					return $"StartAdjustmentType\t{_demo.StartAdjustmentType}";
				case "endtype":
					return $"EndAdjustmentType\t{_demo.EndAdjustmentType}";
				case "timeadj":
					return $"AdjustedTicks\t{_demo.AdjustedTime.ToString("N3")}";
				case "ticksadj":
					return $"AdjustedTicks\t{_demo.AdjustedTicks}";
				case "tickrate":
					return $"Tickrate\t{_demo.Tickrate}";
				case "tps":
					return $"TicksPerSecond\t{_demo.TicksPerSecond.ToString("N3")}";
				case "commands":
					return $"ConsoleCommands\n{string.Join("\n", _demo.ConsoleCommands)}";
				case "packets":
					return $"Packets\n{string.Join("\n", _demo.Packets)}";
				case "game":
					return $"GameInfo\t{_demo.GameInfo}";
				case "gamename":
					return $"GameInfo.Name\t{_demo.GameInfo.Name}";
				case "gamemode":
					return $"GameInfo.Mode\t{_demo.GameInfo.Mode}";
				case "gametickrate":
					return $"GameInfo.DefaultTickrate\t{_demo.GameInfo.DefaultTickrate}";
				case "gamemaps":
					return $"GameInfo.Maps\n{string.Join("\n", _demo.GameInfo.Maps)}";
				default:
					return default(string);
			}
		}
	}
}