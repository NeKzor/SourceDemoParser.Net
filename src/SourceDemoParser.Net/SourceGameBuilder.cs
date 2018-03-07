using System;
using System.Collections.Generic;

namespace SourceDemoParser
{
	public class SourceGameBuilder
	{
		public static readonly Func<SourceDemo, SourceGame> Default = (demo) => new SourceGameBuilder().Build(demo);
		public static readonly Func<SourceDemo, SourceGame> Empty = (_) => SourceGame.Default;

		protected SourceGame _game;

		public SourceGameBuilder()
			=> _game = SourceGame.Default;
		
		public SourceGameBuilder WithMaxSplitscreenClients(int maxSplitscreenClients)
		{
			_game.WithMaxSplitscreenClients(maxSplitscreenClients);
			return this;
		}
		public SourceGameBuilder WithAlignmentByte(bool hasAlignmentByte)
		{
			_game.WithAlignmentByte(hasAlignmentByte);
			return this;
		}
		public SourceGameBuilder WithDefaultDemoMessages(List<DemoMessageType> defaultMessages)
		{
			_game.WithDefaultDemoMessages(defaultMessages);
			return this;
		}
		public SourceGameBuilder WithDefaultNetMessages(List<NetMessageType> defaultNetMessages)
		{
			_game.WithDefaultNetMessages(defaultNetMessages);
			return this;
		}
		
		public virtual SourceGame Build(SourceDemo demo)
		{
			switch (demo.Protocol)
			{
				case 2:
				case 3:
					_game.WithAlignmentByte(false);
					_game.WithDefaultDemoMessages(DemoMessages.OldEngine);
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
					_game.WithMaxSplitscreenClients(2);
					_game.WithDefaultNetMessages(NetMessages.Portal2);
					break;
				case "csgo":
					_game.WithMaxSplitscreenClients(2);
					_game.WithDefaultNetMessages(NetMessages.Csgo);
					break;
			}
			return _game;
		}
	}
}