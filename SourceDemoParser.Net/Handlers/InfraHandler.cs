using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class InfraHandler : OrangeBoxHandler
	{
		private int _endTick = -1;
		private string _endAdjustType;
		private Point3D _lastPosition;
		private readonly Game _gameInfo;

		public InfraHandler(Game supportedGame)
			=> _gameInfo = supportedGame;

		public override SourceDemo GetResult()
		{
			var result = base.GetResult();
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
			return result;
		}

		// TODO: wait for part 3
		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			var result = base.ProcessPacket(br);
			_lastPosition = result.CurrentPosition;
			return result;
		}
	}
}