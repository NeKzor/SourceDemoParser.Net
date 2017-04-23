using System.IO;
using SourceDemoParser.Net.Helpers;
using SourceDemoParser.Net.Results;

namespace SourceDemoParser.Net.Handlers
{
	internal class PortalStoriesMelHandler : OrangeBoxHandler
	{
		private int _startTick = -1;
		private int _endTick = -1;
		private string _startAdjustType;
		private string _endAdjustType;
		private readonly Point3D _startPosition = new Point3D(-4592.00f, -4475.4052734375f, 108.683975219727f);
		private Point3D _lastPosition;
		private readonly Game _gameInfo;

		public PortalStoriesMelHandler()
			=> _gameInfo = SupportedGames.PortalStoriesMel;

		private bool Teleported(Point3D position)
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
				&& ((MapName == "sp_a4_finale") || (MapName == "st_a4_finale"))
				&& (result.Command == "playvideo_exitcommand_nointerrupt aegis_interior.bik end_movie movie_aegis_interior"))
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
				&& ((MapName == "sp_a1_tramride") || (MapName == "st_a1_tramride"))
				&& (Teleported(result.CurrentPosition)))
			{
				_startAdjustType = "Tram Teleportation";
				_startTick = CurrentTick;
			}
			_lastPosition = result.CurrentPosition;
			return result;
		}
	}
}