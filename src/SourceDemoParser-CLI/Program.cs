using System;
using System.IO;
using System.Linq;
using SourceDemoParser;
using SourceDemoParser.Extensions;

namespace SourceDemoParser_CLI
{
	internal static class Program
	{
		private static SourceParser _parser;
		private static SourceDemo _demo;
		private static bool _discovered;

		private static void Main(string[] args)
		{
			if (args != null)
			{
				if (args.Length == 1)
				{
					if (File.Exists(args[0]))
					{
						try
						{
							_parser = new SourceParser();
							_demo = _parser.ParseFileAsync(args[0]).GetAwaiter().GetResult();
							Console.Write(ParseCommand("header"));
						}
						catch (Exception ex)
						{
							Console.Write(ex.ToString());
						}
					}
				}
				else if (args.Length >= 2)
				{
					var file = string.Join(" ", args.Skip(1));
					if (File.Exists(file))
					{
						try
						{
							_parser = new SourceParser();
							_demo = _parser.ParseFileAsync(file).GetAwaiter().GetResult();
							var output = string.Empty;
							foreach (var command in args[0].Split(';'))
							{
								var temp = ParseCommand(command);
								output += (temp != default(string)) ? $"{temp}\n" : string.Empty;
							}
							Console.Write((output != string.Empty) ? output.Substring(0, output.Length - 1) : "Could not parse any commands!");
						}
						catch (Exception ex)
						{
							Console.Write(ex.ToString());
						}
					}
				}
			}
		}

		private static string ParseCommand(string command)
		{
			var count = 0;
			var filter = default(string[]);
			if ((command.StartsWith("messages")
				|| command.StartsWith("commands")
				|| command.StartsWith("packets")) && (command.Contains("=")))
			{
				var args = command.Split('=');
				if (args.Length == 2)
				{
					if (int.TryParse(args[1], out count))
					{
					}
					else if (command.StartsWith("commands"))
					{
						count = 1;
						filter = args[1].Split(',');
					}
				}
				command = command.Substring(0, command.IndexOf("="));
			}
			var start = 0;
			var end = 0;
			if ((command.StartsWith("adj")) && (command.Contains("=")))
			{
				var args = command.Split('=');
				if (args.Length == 2)
				{
					if (int.TryParse(args[1], out end))
					{
					}
					else if (args[1].Contains(","))
					{
						var ticks = args[1].Split(',');
						if (ticks.Length == 2)
						{
							int.TryParse(ticks[0], out start);
							int.TryParse(ticks[1], out end);
						}
					}
				}
				command = command.Substring(0, command.IndexOf("="));
			}
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
					return $"FileStamp\t{_demo.HeaderId}";
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
				case "tps":
				case "ticks-per-second":
					return $"TicksPerSecond\t{_demo.GetTicksPerSecond().ToString("N3")}";
				case "messages":
					return (count > 0) ? $"Messages\n{string.Join("\n", _demo.Messages.Take(count))}"
							   : $"Messages\n{string.Join("\n", _demo.Messages)}";
				case "commands":
					var ccmd = new DemoMessageType("ConsoleCmd");
					return (filter == default(string[]))
						? (count == 0)
							? $"ConsoleCommands\n{string.Join("\n", _demo.GetMessagesByType(ccmd))}"
							: $"ConsoleCommands\n{string.Join("\n", _demo.GetMessagesByType(ccmd).Take(count))}"
						: "ConsoleCommands\n" + string.Join("\n", _demo.GetMessagesByType(ccmd).Where(m =>
						{
							var cmd = (m.Frame as ConsoleCmdFrame).ConsoleCommand;
							foreach (var f in filter)
								if (cmd.StartsWith(f))
									return false;
							return true;
						}));
				case "packets":
					var packet = new DemoMessageType("Packet");
					return (count > 0)
						? $"Packets\n{string.Join("\n", _demo.GetMessagesByType(packet).Take(count))}"
						: $"Packets\n{string.Join("\n", _demo.GetMessagesByType(packet))}";
				// Returns nothing
				case "adj":
				case "adjust":
					_demo.AdjustExact(startTick: start, endTick: end).GetAwaiter().GetResult();
					break;
				case "adj-sf":
				case "adjust-sf":
					_demo.AdjustFlagAsync().GetAwaiter().GetResult();
					break;
				case "adj2":
				case "adjust2":
					if (!_discovered)
					{
						SourceExtensions.DiscoverAsync().GetAwaiter().GetResult();
						_discovered = true;
					}
					_demo.AdjustAsync().GetAwaiter().GetResult();
					break;
			}
			return default(string);
		}
	}
}