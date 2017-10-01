#define EDIT
using System;
using System.Diagnostics;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Test
{
	internal static class Program
	{
		private static void Main()
		{
			ModificationTest();
		}

		[Conditional("EDIT")]
		private static async void ModificationTest()
		{
			const string path = @"..\..\..\Test\";
			const string source = "LaserIntro_Zypeh_1190.dem";
			const string destination = "LaserIntro_Zypeh_1190_edit.dem";

			var parser = new SourceParser(autoAdjustment: false);
			var demo = await parser.ParseFileAsync(path + source);

			Console.WriteLine("Before: " + demo.PlaybackTicks);
			await demo.AdjustExact();

			try
			{
				foreach (var message in demo.Messages)
				{
					if ((message.Frame is ConsoleCmdFrame cf) && (cf.ConsoleCommand.StartsWith("r_flashlight")))
						cf.ConsoleCommand = "echo NeKz was here";

					// Lmao
					if (message.Frame is PacketFrame pf)
						pf.Players[0].Flags = (int)DemoFlags.FDEMO_USE_ANGLES2;
				}

				await demo.ExportFileAsync(path + destination);

				// Parse again
				var edit = await parser.ParseFileAsync(path + destination);

				// Print result
				Console.WriteLine("After: " + demo.PlaybackTicks);
				Console.WriteLine(demo.GetTickrate());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			Console.ReadLine();
		}
	}
}