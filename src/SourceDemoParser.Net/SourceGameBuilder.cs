using System;

namespace SourceDemoParser
{
	public class SourceGameBuilder
	{
		public static readonly Func<SourceDemo, SourceGame> Default = (demo) => new SourceGameBuilder().Build(demo);
		public static readonly Func<SourceDemo, SourceGame> Empty = (_) => SourceGame.Default;

		protected SourceGame _game;

		public SourceGameBuilder()
			=> _game = SourceGame.Default;
		
		public virtual SourceGame Build(SourceDemo demo)
		{
			switch (demo.Protocol)
			{
				case 2:
				case 3:
					_game.HasAlignmentByte = false;
					_game.DefaultMessages = DemoMessages.OldEngine;
					break;
				case 4:
					break;
				default:
					throw new ProtocolException(demo.Protocol);
			}
			switch (demo.GameDirectory)
			{
				case "portal2":
				case "aperturetag":
				case "portal_stories":
				case "infra":
					_game.MaxSplitscreenClients = 2;
					_game.DefaultNetMessages = NetMessages.Portal2;
					break;
			}
			return _game;
		}
	}
}