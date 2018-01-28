using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if DISCOVER_2
using System.Reflection;
#endif
using SourceDemoParser;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Test
{
	internal static class Program
	{
		private const string path = "demos/";

		private static void Main()
		{
			ParsingTest();
			TimingTest();
			ModificationTest();
			DiscoverTest();
			DirectLoadTest();
			Cleanup();
			OldEngine();
		}

		[Conditional("PARSE")]
		private static void ParsingTest()
		{
			const string source = "portal2_sp.dem";

			var parser = new SourceParser();
			var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();
#if EDIT
			var count = 0;
			foreach (var frame in demo.Messages.Select(m => m.Frame))
			{
				if (frame is UserCmdFrame uf)
				{
					count++;
					var data = frame.RawData;
					var export = frame.ExportData().GetAwaiter().GetResult();

					using (var br = new BinaryReader(new MemoryStream(export)))
					{
						var num = br.ReadInt32();
						var length = br.ReadInt32();
						var raw = br.ReadBytes(length);

						var compare = new UserCmdFrame(num, raw);

						_ = (compare as IFrame).ParseData();

						Debug.WriteLineIf(uf.RawData.Length == compare.RawData.Length, $"[{count}] Same size!");
						Debug.WriteLineIf(uf.RawData.Length != compare.RawData.Length, $"[{count}] Shortened size!");
					}
				}
			}
#endif
				}

		[Conditional("PARSE_T")]
		private static void TimingTest()
		{
			const string source = "portal2_sp.dem";

			var watch = new Stopwatch();
			var demo = new SourceDemo();

			var fast = new SourceParser(ParsingMode.Default);
			var slow = new SourceParser(ParsingMode.Everything);
			var headeronly = new SourceParser(ParsingMode.HeaderOnly);

			// Default
			watch = Stopwatch.StartNew();
			demo = fast.ParseFileAsync(path + source).GetAwaiter().GetResult();
			watch.Stop();
			var result1 = watch.Elapsed.TotalMilliseconds;
			Console.WriteLine("Default: " + watch.Elapsed.TotalMilliseconds.ToString() + "ms");

			// Everything
			watch = Stopwatch.StartNew();
			demo = slow.ParseFileAsync(path + source).GetAwaiter().GetResult();
			watch.Stop();
			var result2 = watch.Elapsed.TotalMilliseconds;
			Console.WriteLine($"Everything: {result2}ms ({(int)(result2 / result1)} times slower)");

			// Header only
			watch = Stopwatch.StartNew();
			demo = headeronly.ParseFileAsync(path + source).GetAwaiter().GetResult();
			watch.Stop();
			var result3 = watch.Elapsed.TotalMilliseconds;
			Console.WriteLine($"Header only: {result3}ms ({(int)(result1 / result3)} times faster)");

			/*	Random Result
				Default: 30.8731ms
				Everything: 63.1461ms (2 times slower)
				Header only: 0.123ms (251 times faster)
			*/
		}

		[Conditional("CLEANUP")]
		private static void Cleanup()
		{
			const string source = "portal2_pausing.dem";

			var parser = new SourceParser();
			var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();

			bool RemovePacket = false;
			var copy = demo.Messages.ToArray();
			demo.Messages.CopyTo(copy);

			var index = 0;
			foreach (var msg in copy)
			{
				if (msg.CurrentTick > 0)
				{
					if (msg.Frame is ConsoleCmdFrame ccf)
					{
						if (ccf.ConsoleCommand == "gameui_activate")
							RemovePacket = true;
						else if (ccf.ConsoleCommand == "gameui_hide")
							RemovePacket = false;
					}
					else if ((msg.Frame is UserCmdFrame) || (msg.Frame is PacketFrame))
					{
						if (RemovePacket)
						{
							demo.Messages.RemoveAt(index);
							index--;
						}
					}
				}
				index++;
			}

			var exporter = new SourceExporter();
			_ = exporter.ExportFileAsync(demo, path + "portal2_cleanup.dem");
		}

		[Conditional("EDIT")]
		private static void ModificationTest()
		{
			const string source = "portal2_sp.dem";
			const string destination = "portal2_sp_edit";

			var parser = new SourceParser();
			var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();

			Console.WriteLine("Before: " + demo.PlaybackTicks);
			_ = demo.AdjustExact();

			try
			{
				foreach (var message in demo.Messages)
				{
					if ((message.Frame is ConsoleCmdFrame cf) && (cf.ConsoleCommand.StartsWith("r_flashlight")))
						cf.ConsoleCommand = "echo NeKz was here";

					if (message.Frame is PacketFrame pf)
					{
						// Lmao
						pf.Infos[0].Flags &= DemoFlags.FDEMO_USE_ANGLES2;
					}
				}

				var exporter = new SourceExporter();

				//_ = exporter.ExportFileAsync(demo, path + destination + "_fast_export.dem", fastExport: true);
				_ = exporter.ExportFileAsync(demo, path + destination + "_export.dem");

				/*	Results when removing messages (tested with Portal 2)

					 yes = works fine
					 no = won't play
					 NOPE = game crash

					 SignOn -> NOPE
					 SyncTick-> yes
					 Stop -> yes
					 UserCmd -> yes
					 Packet -> no
					 ConsoleCmd -> yes
					 CustomData -> yes
					 DataTables -> NOPE
					 StringTables -> yes
				*/

				// Parse again
				var edit = parser.ParseFileAsync(path + destination + "_export.dem").GetAwaiter().GetResult();

				// Print result
				Console.WriteLine("After: " + demo.PlaybackTicks);
				Console.WriteLine(demo.GetTickrate());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		[Conditional("DISCOVER"), Conditional("DISCOVER_2")]
		private static void DiscoverTest()
		{
			try
			{
				const string source = "portal2_cm_coop.dem";

				var parser = new SourceParser();
				var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();
				Console.WriteLine("Before: " + demo.PlaybackTicks);

				// Load automatically from assembly
#if !DISCOVER_2
				_ = SourceExtensions.DiscoverAsync();
#else
				_ = SourceExtensions.DiscoverAsync(Assembly.GetEntryAssembly());
#endif
				_ = demo.AdjustAsync();
				Console.WriteLine("After: " + demo.PlaybackTicks);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		[Conditional("DIRECT")]
		private static void DirectLoadTest()
		{
			try
			{
				const string source = "portal2_cm_coop.dem";

				var parser = new SourceParser();
				var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();
				Console.WriteLine("Before: " + demo.PlaybackTicks);

				// Load custom demo directly
				_ = SourceExtensions.LoadAsync<CustomDemo>();
				_ = demo.AdjustAsync();
				Console.WriteLine("After: " + demo.PlaybackTicks);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		[Conditional("OE")]
		private static void OldEngine()
		{
			const string source = "hl2oe.dem";
			var parser = new SourceParser();
			var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();
			Console.WriteLine($"Messages: {demo.Messages.Count}");
		}
	}

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
}