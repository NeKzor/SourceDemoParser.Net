using System;
using System.IO;
using SourceDemoParser_CLI.Helpers;
using SourceDemoParser_CLI.Results;

namespace SourceDemoParser_CLI.Handlers
{
	internal class Portal2SinglePlayerHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private const string _mainStartAdjustmentType = "Crosshair Appear";
		private const string _mainEndAdjustmentType = "Crosshair Disappear";
		private Game _gameInfo;
		private readonly Point3D _introStartPosition = new Point3D(-8709.20f, 1690.07f, 28.00f);
		private readonly Point3D _introStartPositionTolerance = new Point3D(0.02f, 0.02f, 0.5f);
		private readonly int _introStartTickOffset = 1;
		// Best guess. You can move at ~2-3 units/tick, so don't check exactly.
		private readonly Point3D _finaleEndPosition = new Point3D(54.1f, 159.2f, -201.4f);
		// How many ticks from last portal shot to being at the checkpoint.
		// Experimentally determined, may be wrong.
		private readonly int _finaleEndTickOffset = -852;

		public Portal2SinglePlayerHandler(Game supportedGame)
			=> _gameInfo = supportedGame;

		// Check if you're in a specific cylinder of volume and far enough below the floor.
		private bool OnTheMoon(Point3D position)
			=> (Math.Pow(position.X - _finaleEndPosition.X, 2) + Math.Pow(position.Y - _finaleEndPosition.Y, 2)) < Math.Pow(50, 2)
			&& (position.Z < _finaleEndPosition.Z);

		// Check if at the spawn coordinate for sp_a1_intro1
		private bool AtSpawn(Point3D position)
			=> !(Math.Abs(position.X - _introStartPosition.X) > _introStartPositionTolerance.X)
			&& !(Math.Abs(position.Y - _introStartPosition.Y) > _introStartPositionTolerance.Y)
			&& !(Math.Abs(position.Z - _introStartPosition.Z) > _introStartPositionTolerance.Z);

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
			if (result.Command == "echo #SAVE#")
			{
				_endAdjustType = "#SAVE# Flag";
				_endTick = CurrentTick;
			}
			return result;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			if ((_startAdjustType == null)
				&& (MapName == "sp_a1_intro1")
				&& (AtSpawn(result.CurrentPosition)))
			{
				_startAdjustType = _mainStartAdjustmentType;
				_startTick = CurrentTick + _introStartTickOffset;
			}
			else if ((_endAdjustType == null)
				&& (MapName == "sp_a4_finale4")
				&& (OnTheMoon(result.CurrentPosition)))
			{
				_endAdjustType = _mainEndAdjustmentType;
				_endTick = CurrentTick + _finaleEndTickOffset;
			}
			return result;
		}
	}
}