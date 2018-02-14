using System;
using System.Threading.Tasks;

namespace SourceDemoParser.Tests
{
	internal class Game
	{
		public async Task OldEngine()
		{
			const string source = "hl2oe.dem";
			var parser = new SourceParser();
			var demo = await parser.ParseFileAsync(Paths.Demos + source);
			Console.WriteLine($"Messages: {demo.Messages.Count}");
		}
		public async Task Portal()
		{
			const string source = "portal.dem";
			var parser = new SourceParser(ParsingMode.Everything);
			var demo = await parser.ParseFileAsync(Paths.Demos + source);
			/* foreach (var packet in demo.GetMessagesByType("Packet"))
			{
				var messages = (packet.Frame as PacketFrame).NetMessages;
				if (messages.Count == 0)
				{
					Console.WriteLine($"Skipped a packet at {packet.CurrentTick}");
					continue;
				}
				foreach (var message in messages)
					Console.WriteLine($"Net message: {message}");
			} */
		}
		public async Task Portal2()
		{
			const string source = "portal2_sp.dem";
			var parser = new SourceParser(ParsingMode.Everything);
			var demo = await parser.ParseFileAsync(Paths.Demos + source);
		}
	}
}