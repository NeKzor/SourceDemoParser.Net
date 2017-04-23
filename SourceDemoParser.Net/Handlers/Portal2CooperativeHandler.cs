using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class Portal2CooperativeHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private readonly Game _gameInfo;

		public Portal2CooperativeHandler()
			=> _gameInfo = SupportedGames.Portal2Cooperative;

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
				_startAdjustType = "Cooperative Standard Start";
				_startTick = CurrentTick;
			}
			else if ((_endAdjustType == null)
				&& (CurrentTick > 0))
			{
				if (result.Command == "playvideo_end_level_transition")
				{
					_endAdjustType = "Cooperative Standard End";
					_endTick = CurrentTick;
				}
				else if ((result.Command == "playvideo_exitcommand_nointerrupt coop_outro end_movie vault-movie_outro")
					&& (MapName == "mp_coop_paint_longjump_intro"))
				{
					_endAdjustType = "Cooperative Ending Credits";
					_endTick = CurrentTick;
				}
			}
			return result;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			if ((MapName == "mp_coop_start")
				&& (result.CurrentPosition.Equals(new Point3D(-9896f, -4400f, 3048f))))
			{
				_startAdjustType = "Cooperative Atlas Gain Control";
				_startTick = CurrentTick;
			}
			else if ((MapName == "mp_coop_start")
				&& (result.CurrentPosition.Equals(new Point3D(-11168f, -4384f, 3040.03125f))))
			{
				_startAdjustType = "Cooperative P-Body Gain Control";
				_startTick = CurrentTick;
			}
			return result;
		}
	}
}