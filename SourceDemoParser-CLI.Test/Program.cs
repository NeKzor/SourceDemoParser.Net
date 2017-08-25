using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SourceDemoParser_CLI.Test
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var app = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var exe = app + "/SourceDemoParser-CLI.exe";
			var demo = app + "/LaserOverGoo_NeKz_0p.dem";
			if (File.Exists(exe))
			{
				if (File.Exists(demo))
				{
					var result = Process.Start(new ProcessStartInfo
					{
						FileName = exe,
						Arguments = "parse " + demo,
						UseShellExecute = false,
						RedirectStandardOutput = true
					}).StandardOutput.ReadToEnd();
					Console.WriteLine(result);
				}
				else
					throw new Exception("Where's the demo lmao?");
			}
			else
				throw new Exception("Where's the tool xd???");
		}
	}
}
