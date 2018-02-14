using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SourceDemoParser.Tests
{
	internal class Splicer
	{
		public async Task Splice()
		{
			const string source1 = "seg1.dem";
			const string source2 = "seg2.dem";
			const string output = "splice.dem";

			var parser = new SourceParser(ParsingMode.Default, AdjustmentType.Exact);
			var exporter = new SourceExporter(ExportMode.Default);

			var demo1 = await parser.ParseFileAsync(Paths.Demos + source1);
			var demo2 = await parser.ParseFileAsync(Paths.Demos + source2);

			Console.WriteLine("Ticks before: " + demo1.PlaybackTicks);

			demo1.PlaybackTicks += demo2.PlaybackTicks;
			demo1.PlaybackTime += demo2.PlaybackTime;
			demo1.PlaybackFrames += demo2.PlaybackFrames;

			Console.WriteLine("Ticks after: " + demo1.PlaybackTicks);
			Console.WriteLine("Messages before: " + demo1.Messages.Count);

			demo1.Messages.RemoveAll(m => m.Type.Name == "Stop");

			var range = demo2.Messages.Where(m =>
			{
				if ((m.Tick == 0) && (m.Frame is ConsoleCmdFrame))
					return false;
				
				return (m.Frame is PacketFrame)
					|| (m.Frame is DataTablesFrame)
					|| (m.Frame is UserCmdFrame)
					|| (m.Frame is ConsoleCmdFrame)
					|| (m.Frame is StopFrame);
			});

			var tick = demo1.Messages.Last().Tick;
			Console.WriteLine("Rebase from: " + tick);

			foreach (var msg in range)
				msg.Tick += tick;

			demo1.Messages.AddRange(range);
			Console.WriteLine("Messages after: " + demo1.Messages.Count);

			await exporter.ExportFileAsync(demo1, output);
			var demo3 = await parser.ParseFileAsync(output);

			using (var fs = File.Create("log5.txt"))
			using (var sr = new StreamWriter(fs))
			{
				foreach (var msg in demo3.Messages)
				{
					sr.WriteLine(msg);
				}
			}
		}
		public async Task Analyze()
		{
			const string source = "seg2.dem";
			var parser = new SourceParser(ParsingMode.Everything, AdjustmentType.Exact);
			var demo = await parser.ParseFileAsync(Paths.Demos + source);
			
			using (var fs = File.Create("log3.txt"))
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