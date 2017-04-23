using System;
using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class Portal2SinglePlayerHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private Point3D _lastPosition;
		private readonly Point3D _introStartPosition = new Point3D(-8709.20f, 1690.07f, 28.00f);
		private readonly Point3D _introStartPositionTolerance = new Point3D(0.02f, 0.02f, 0.5f);
		private readonly Point3D _finaleEndPosition = new Point3D(54.1f, 159.2f, -201.4f);
		private readonly Point3D _e1912StartPosition = new Point3D(-655.748779296875f, -918.37353515625f, -4.96875f);
		private readonly int _introStartTickOffset = 1;
		private readonly int _finaleEndTickOffset = -852;
		private readonly int _bonusMapStartTickOffset = -2;
		private readonly Game _gameInfo;

		public Portal2SinglePlayerHandler(Game supportedGame)
			=> _gameInfo = supportedGame;

		private bool OnTheMoon(Point3D position)
			=> (Math.Pow(position.X - _finaleEndPosition.X, 2) + Math.Pow(position.Y - _finaleEndPosition.Y, 2)) < Math.Pow(50, 2)
			&& (position.Z < _finaleEndPosition.Z);

		private bool AtSpawn(Point3D position)
			=> !(Math.Abs(position.X - _introStartPosition.X) > _introStartPositionTolerance.X)
			&& !(Math.Abs(position.Y - _introStartPosition.Y) > _introStartPositionTolerance.Y)
			&& !(Math.Abs(position.Z - _introStartPosition.Z) > _introStartPositionTolerance.Z);

		private bool BonusMapStart(Point3D position)
			=> (_lastPosition.Equals(_e1912StartPosition))
			&& (!(position.Equals(_e1912StartPosition)));

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
				_startAdjustType = "Gain Control";
				_startTick = CurrentTick + _introStartTickOffset;
			}
			else if ((_startAdjustType == null)
				&& (MapName == "e1912")
				&& (BonusMapStart(result.CurrentPosition)))
			{
				_startAdjustType = "Registered Input";
				_startTick = CurrentTick + _bonusMapStartTickOffset;
			}
			else if ((_endAdjustType == null)
				&& (MapName == "sp_a4_finale4")
				&& (OnTheMoon(result.CurrentPosition)))
			{
				_endAdjustType = "Moon Shot";
				_endTick = CurrentTick + _finaleEndTickOffset;
			}
			_lastPosition = result.CurrentPosition;
			return result;
		}
	}
}