using System;
using System.Diagnostics;

namespace SourceDemoParser_CLI.Test
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var result = Process.Start(new ProcessStartInfo
			{
				FileName = @"..\..\..\SourceDemoParser-CLI\bin\Debug\SourceDemoParser-CLI.exe",
				Arguments = @"game;tickrate;ticks;time ..\..\LaserOverGoo_NeKz_0p.dem",
				UseShellExecute = false,
				RedirectStandardOutput = true
			}).StandardOutput.ReadToEnd();
			Console.WriteLine(result);
			Console.ReadKey();
		}
	}
}