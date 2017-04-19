using System;
using System.Linq;

namespace SourceDemoParser_CLI
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			if ((args != null) && (args?.Length >= 2))
			{
				try
				{
					var demo = SourceDemoParser.Parse(string.Join(" ", args.Skip(1))).GetAwaiter().GetResult();
					if (demo != default(SourceDemo))
					{
						switch (args[0])
						{
							case "parse":
								Console.Write($"GameInfo\t{demo.GameInfo}\n" +
									$"DemoProtocol\t{demo.DemoProtocol}\n" +
									$"NetworkProtocol\t{demo.NetworkProtocol}\n" +
									$"GameDir\t{demo.GameDirectory}\n" +
									$"FilePath\t{demo.FilePath}\n" +
									$"GameDirectory\t{demo.GameDirectory}\n" +
									$"MapName\t{demo.MapName}\n" +
									$"PlayerName\t{demo.Client}\n" +
									$"PlaybackTime\t{demo.PlaybackTime}\n" +
									$"PlaybackTicks\t{demo.PlaybackTicks}\n" +
									$"FrameCount\t{demo.FrameCount}\n" +
									$"StartAdjustmentTick\t{demo.StartAdjustmentTick}\n" +
									$"EndAdjustmentTick\t{demo.EndAdjustmentTick}\n" +
									$"StartAdjustmentType\t{demo.StartAdjustmentType}\n" +
									$"EndAdjustmentType\t{demo.EndAdjustmentType}\n" +
									$"AdjustedTicks\t{demo.AdjustedTicks}");
								break;
							case "version":
								Console.Write($"DemoProtocol\t{demo.DemoProtocol}");
								break;
							case "netproc":
								Console.Write($"NetworkProtocol\t{demo.NetworkProtocol}");
								break;
							case "path":
								Console.Write($"FilePath\t{demo.FilePath}");
								break;
							case "dir":
								Console.Write($"GameDirectory\t{demo.GameDirectory}");
								break;
							case "map":
								Console.Write($"MapName\t{demo.MapName}");
								break;
							case "client":
								Console.Write($"Client\t{demo.Client}");
								break;
							case "time":
								Console.Write($"PlaybackTime\t{demo.PlaybackTime.ToString("N3")}");
								break;
							case "ticks":
								Console.Write($"PlaybackTicks\t{demo.PlaybackTicks}");
								break;
							case "frames":
								Console.Write($"FrameCount\t{demo.FrameCount}");
								break;
							case "signon":
								Console.Write($"SignOnLength\t{demo.SignOnLength}");
								break;
							case "tickstart":
								Console.Write($"StartAdjustmentTick\t{demo.StartAdjustmentTick}");
								break;
							case "tickend":
								Console.Write($"EndAdjustmentTick\t{demo.EndAdjustmentTick}");
								break;
							case "starttype":
								Console.Write($"StartAdjustmentType\t{demo.StartAdjustmentType}");
								break;
							case "endtype":
								Console.Write($"EndAdjustmentType\t{demo.EndAdjustmentType}");
								break;
							case "adjusted":
								Console.Write($"AdjustedTicks\t{demo.AdjustedTicks}");
								break;
							case "tickrate":
								Console.Write($"Tickrate\t{demo.Tickrate}");
								break;
							case "tps":
								Console.Write($"TicksPerSecond\t{demo.TicksPerSecond.ToString("N3")}");
								break;
							case "commands":
								Console.Write($"ConsoleCommands\n{string.Join("\n", demo.ConsoleCommands)}");
								break;
							case "packets":
								Console.Write($"Packets\n{string.Join("\n", demo.Packets)}");
								break;
							case "game":
								Console.Write($"GameInfo\t{demo.GameInfo}");
								break;
							case "gamename":
								Console.Write($"GameInfo.Name\t{demo.GameInfo.Name}");
								break;
							case "gamemode":
								Console.Write($"GameInfo.Mode\t{demo.GameInfo.Mode}");
								break;
							case "gametickrate":
								Console.Write($"GameInfo.DefaultTickrate\t{demo.GameInfo.DefaultTickrate}");
								break;
							case "gamemaps":
								Console.Write($"GameInfo.Maps\n{string.Join("\n", demo.GameInfo.Maps)}");
								break;
							default:
								Console.WriteLine("Unknown command!");
								break;
						}
					}
				}
				catch
				{
					// Unsupported demo file (probably)
				}
			}
		}
	}
}