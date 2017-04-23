using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class PortalHandler : HL2Handler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private readonly Game _gameInfo;

		public PortalHandler()
			=> _gameInfo = SupportedGames.Portal;

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
			if ((_endAdjustType == null)
				&& (result.Command == "echo #SAVE#"))
			{
				_endAdjustType = "#SAVE# Flag";
				_endTick = CurrentTick;
			}
			else if ((_endAdjustType == null)
				&& (MapName == "escape_02")
				&& (result.Command == "startneurotoxins 99999"))
			{
				_endAdjustType = "Beat GLaDOS";
				_endTick = CurrentTick + 1;
			}
			return result;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			if ((_startAdjustType == null)
				&& (MapName == "testchmb_a_00")
				&& (result.CurrentPosition.Equals(new Point3D(-544f, -368.75f, 160f))))
			{
				_startAdjustType = "Gain Control";
				_startTick = CurrentTick + 1;
			}
			return result;
		}
	}
}