using System.IO;
using SourceDemoParser_CLI.Helpers;
using SourceDemoParser_CLI.Results;

namespace SourceDemoParser_CLI.Handlers
{
	internal class Portal2CooperativeDlcHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private Game _gameInfo;

		public Portal2CooperativeDlcHandler()
			=> _gameInfo = Game.Portal2CooperativeDlc;

		public override SourceDemo GetResult()
		{
			var result = base.GetResult();
			if (_startAdjustType != null)
			{
				result.StartAdjustmentType = _startAdjustType;
				result.StartAdjustmentTick = _startTick;
			}
			if (_endAdjustType != null)
			{
				result.EndAdjustmentType = _endAdjustType;
				result.EndAdjustmentTick = _endTick;
			}
			result.GameInfo = _gameInfo;
			return result;
		}

		protected override ConsoleCmdResult ProcessConsoleCmd(BinaryReader br)
		{
			var result = base.ProcessConsoleCmd(br);
			if ((_startAdjustType == null)
				&& (CurrentTick > 0)
				&& (result.Command == "ss_force_primary_fullscreen 0"))
			{
				_startAdjustType = "Portal2 co-op course 6 Start Standard";
				_startTick = CurrentTick;
			}
			if ((_endAdjustType == null)
				&& (CurrentTick > 0))
			{
				if ((result.Command.StartsWith("playvideo_end_level_transition")) && (MapName != "mp_coop_paint_crazy_box"))
				{
					_endAdjustType = "Portal2 co-op course 6 End Standard";
					_endTick = CurrentTick;
				}
				else if (result.Command == "playvideo_exitcommand_nointerrupt dlc1_endmovie end_movie movie_outro" && MapName == "mp_coop_paint_crazy_box")
				{
					_endAdjustType = "Portal2 co-op course 6 Run End";
					_endTick = CurrentTick;
				}
			}
			return result;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			if ((_startAdjustType == null)
				&& (result.CurrentPosition.Equals(new Point3D(-9896f, -4400f, 3048f))))
			{
				_startAdjustType = "Portal2 co-op Atlas Gain Control";
				_startTick = CurrentTick;
			}
			else if (result.CurrentPosition.Equals(new Point3D(-11168f, -4384f, 3040.03125f)))
			{
				_startAdjustType = "Portal2 co-op P-Body Gain Control";
				_startTick = CurrentTick;
			}
			return result;
		}
	}
}