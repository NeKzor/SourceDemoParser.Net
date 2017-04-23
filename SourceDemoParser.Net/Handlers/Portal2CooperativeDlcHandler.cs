using System.IO;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class Portal2CooperativeDlcHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private readonly Game _gameInfo;

		public Portal2CooperativeDlcHandler()
			=> _gameInfo = SupportedGames.Portal2CooperativeDlc;

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
				_startAdjustType = "Cooperative DLC Standard Start";
				_startTick = CurrentTick;
			}
			else if ((_endAdjustType == null)
				&& (CurrentTick > 0))
			{
				if (result.Command == "playvideo_end_level_transition")
				{
					_endAdjustType = "Cooperative DLC Standard End";
					_endTick = CurrentTick;
				}
				else if (result.Command == "playvideo_exitcommand_nointerrupt dlc1_endmovie end_movie movie_outro"
					&& MapName == "mp_coop_paint_crazy_box")
				{
					_endAdjustType = "Cooperative DLC Ending Credits";
					_endTick = CurrentTick;
				}
			}
			return result;
		}
	}
}