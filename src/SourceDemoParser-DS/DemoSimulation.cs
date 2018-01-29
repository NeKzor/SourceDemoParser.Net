using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using SourceDemoParser;
using SourceDemoParser.Extensions;

namespace SourceDemoParser_DS
{
	public class DemoSimulation
	{
		public SourceDemo Demo { get; set; }
		private int _currentTick;

		public DemoSimulation(string file)
		{
			var parser = new SourceParser(ParsingMode.Everything);
			Demo = parser.ParseFileAsync(file).GetAwaiter().GetResult();
			Demo.AdjustExact().GetAwaiter().GetResult();
		}

		public void SetTick(int tick)
			=> _currentTick = tick;
		public float GetTps()
			=> Demo.GetTicksPerSecond();

		public (List<string> Commands, List<PacketInfo> Info, List<UserCmd> User) GetNextData()
		{
			var messages = Demo.GetMessagesByTick(_currentTick);

			var commands = new List<string>();
			var infos = new List<PacketInfo>();
			var usercmds = new List<UserCmd>();

			foreach (var msg in messages)
			{
				if (msg.Frame is ConsoleCmdFrame cf)
					commands.Add(cf.ConsoleCommand);
				else if (msg.Frame is PacketFrame pf)
					infos.Add(pf.Infos[0]);
				else if (msg.Frame is UserCmdFrame uf)
					usercmds.Add(uf.Cmd);
			}

			_currentTick++;
			if (_currentTick > Demo.PlaybackTicks)
				_currentTick = 0;

			return (commands, infos, usercmds);
		}

		public (DataPoint Yz, DataPoint Xz, DataPoint Xy) GetStartingPoints()
		{
			var info = Demo
				.GetMessagesByType("Packet")
				.Select(m => m.Frame)
				.Cast<PacketFrame>()
				.Select(f => f.Infos[0])
				.First();
			return (new DataPoint(info.ViewOrigin.Y, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Y));
		}

		public (DataPoint Yz, DataPoint Xz, DataPoint Xy) GetEndingPoints()
		{
			var info = Demo
				.GetMessagesByType("Packet")
				.Select(m => m.Frame)
				.Cast<PacketFrame>()
				.Select(f => f.Infos[0])
				.Last();
			return (new DataPoint(info.ViewOrigin.Y, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Y));
		}
	}

	public enum KeyType
	{
		W,
		A,
		S,
		D
	}

	public static class DemoSimulationExtensions
	{
		public static string GetCommands(this IEnumerable<string> commands)
		{
			return string.Join("\n", commands);
		}
		public static (DataPoint Yz, DataPoint Xz, DataPoint Xy, DataPoint Mxy) GetPoints(this PacketInfo info)
		{
			return (new DataPoint(info.ViewOrigin.Y, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Z),
					new DataPoint(info.ViewOrigin.X, info.ViewOrigin.Y),
					new DataPoint((float)info.ViewAngles.X, (float)info.ViewAngles.Y));
		}
		public static bool IsKeyPressed(this UserCmd user, KeyType key)
		{
			switch (key)
			{
				case KeyType.W:
					return user.ForwardMove > 0;
				case KeyType.A:
					return user.SideMove > 0;
				case KeyType.S:
					return user.ForwardMove < 0;
				case KeyType.D:
					return user.SideMove < 0;
				default:
					return false;
			}
		}
		public static DataPoint GetNewMousePoint(this UserCmd user, DataPoint oldMxy)
		{
			var x = oldMxy.X;
			var y = oldMxy.Y;
			if (user.MouseDx != null)
				x += (double)user.MouseDx;
			if (user.MouseDy != null)
				y += (double)user.MouseDy;
			return new DataPoint(x, y);
		}
	}
}