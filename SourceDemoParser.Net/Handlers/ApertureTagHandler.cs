using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class ApertureTagHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private Point3D _lastPosition;
		private readonly Point3D _startPosition = new Point3D(-723.00f, -2481.00f, 17.00f);
		private readonly Game _gameInfo;

		public ApertureTagHandler(Game supportedGame)
			=> _gameInfo = supportedGame;

		private bool IsMoving(Point3D position)
			=> (_startPosition.Equals(_lastPosition))
			&& (!(_startPosition.Equals(position)));

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
			return result;
		}

		protected override ConsoleCmdResult ProcessConsoleCmd(BinaryReader br)
		{
			var result = base.ProcessConsoleCmd(br);
			if ((_endAdjustType == null)
				&& (result.Command == "echo #SAVE#"))
			{
				_endAdjustType = "#SAVE# Flag";
				_endTick = CurrentTick;
			}
			else if ((_endAdjustType == null)
				&& (MapName == "gg_stage_theend")
				&& (result.Command == "playvideo_exitcommand_nointerrupt at_credits end_movie credits_video"))
			{
				_endAdjustType = "Ending Credits";
				_endTick = CurrentTick;
			}
			return result;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			if ((_startAdjustType == null)
				&& (MapName == "gg_intro_wakeup")
				&& (IsMoving(result.CurrentPosition)))
			{
				_startAdjustType = "Gain Control";
				_startTick = CurrentTick;
			}
			_lastPosition = result.CurrentPosition;
			return result;
		}
	}
}