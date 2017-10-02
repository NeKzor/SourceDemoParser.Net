//#define EDIT
#define DISCOVER_2
//#define DIRECT
using System;
using System.Diagnostics;
using System.Reflection;
using SourceDemoParser.Extensions;

namespace SourceDemoParser.Test
{
	internal static class Program
	{
		private static void Main()
		{
			ModificationTest();
			DiscoverTest();
			DirectLoadTest();
		}

		[Conditional("EDIT")]
		private static void ModificationTest()
		{
			const string path = @"..\Test\";
			const string source = "LaserIntro_Zypeh_1190.dem";
			const string destination = "LaserIntro_Zypeh_1190_edit.dem";

			var parser = new SourceParser(autoAdjustment: false);
			var demo = parser.ParseFileAsync(path + source).GetAwaiter().GetResult();

			Console.WriteLine("Before: " + demo.PlaybackTicks);
			_ = demo.AdjustExact();

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

				_ = demo.ExportFileAsync(path + destination);

				// Parse again
				var edit = parser.ParseFileAsync(path + destination).GetAwaiter().GetResult();

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

		[Conditional("DISCOVER"), Conditional("DISCOVER_2")]
		private static void DiscoverTest()
		{
			try
			{
				const string path = @"..\Test\";
				const string source = "TeamRetrieval_1596_Zypeh.dem";

				var parser = new SourceParser(autoAdjustment: false);
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
			Console.ReadLine();
		}

		[Conditional("DIRECT")]
		private static void DirectLoadTest()
		{
			try
			{
				const string path = @"..\Test\";
				const string source = "TeamRetrieval_1596_Zypeh.dem";

				var parser = new SourceParser(autoAdjustment: false);
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
			Console.ReadLine();
		}
	}

	public class NonStatic
	{
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