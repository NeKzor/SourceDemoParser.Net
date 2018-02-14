using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

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
			var demo = await parser.ParseFileAsync(Paths.Demos + source);
		}
		// Note: Bad benchmarking example
		public async Task Timing()
		{
			const string source = "portal2_sp.dem";

			var watch = new Stopwatch();
			var demo = new SourceDemo();

			var fast = new SourceParser(ParsingMode.Default);
			var slow = new SourceParser(ParsingMode.Everything);
			var headeronly = new SourceParser(ParsingMode.HeaderOnly);

			// Default
			watch = Stopwatch.StartNew();
			demo = await fast.ParseFileAsync(Paths.Demos + source);
			watch.Stop();
			var result1 = watch.Elapsed.TotalMilliseconds;
			Console.WriteLine("Default: " + watch.Elapsed.TotalMilliseconds.ToString() + "ms");

			// Everything
			watch = Stopwatch.StartNew();
			demo = await slow.ParseFileAsync(Paths.Demos + source);
			watch.Stop();
			var result2 = watch.Elapsed.TotalMilliseconds;
			Console.WriteLine($"Everything: {result2}ms ({(int)(result2 / result1)} times slower)");

			// Header only
			watch = Stopwatch.StartNew();
			demo = await headeronly.ParseFileAsync(Paths.Demos + source);
			watch.Stop();
			var result3 = watch.Elapsed.TotalMilliseconds;
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

			var parser = new SourceParser(ParsingMode.Default, AdjustmentType.None);
			var exporter = new SourceExporter(ExportMode.Default);

			var demo = await parser.ParseFileAsync(Paths.Demos + source);
			var demo2 = default(SourceDemo);

			await exporter.ExportFileAsync(demo, destination);
			try
			{
				
				/* foreach (var msg in demo.Messages)
				{
					using (var ms = new MemoryStream())
					using (var bw = new BinaryWriter(ms, Encoding.ASCII))
					{
						await msg.Export(bw, demo);
						if (msg.Frame is PacketFrame pf)
						{
							var expected = pf.PacketData.Length + pf.NetData.Length + 4;
							if (expected != bw.BaseStream.Length)
								throw new Exception("Wat?");
						}
					}
				} */
				demo2 = await parser.ParseFileAsync(destination);
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
				var demo = await parser.ParseFileAsync(Paths.Demos + source);
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
				var demo = await parser.ParseFileAsync(Paths.Demos + source);
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
			var demo = await parser.ParseFileAsync(Paths.Demos + source);

			bool RemovePacket = false;
			var copy = demo.Messages.ToArray();
			demo.Messages.CopyTo(copy);

			var index = 0;
			foreach (var msg in copy)
			{
				if (msg.Tick > 0)
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
			await exporter.ExportFileAsync(demo, Paths.Demos + "portal2_cleanup.dem");
		}
	}
}