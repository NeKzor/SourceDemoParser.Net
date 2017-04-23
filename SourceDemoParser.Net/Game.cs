using System.Collections.Generic;
using System.Linq;

namespace SourceDemoParser.Net
{
	public class Game
	{
		public string Name { get; }
		public string Mode { get; }
		public int DefaultTickrate { get; }
		public IEnumerable<Map> Maps { get; }

		public Game()
		{
		}
		public Game(string name, int tickrate = default(int), IEnumerable<Map> maps = default(IEnumerable<Map>))
		{
			Name = name;
			DefaultTickrate = tickrate;
			Maps = maps;
		}
		public Game(string name, string mode, int tickrate, IEnumerable<Map> maps = default(IEnumerable<Map>))
		{
			Name = name;
			Mode = mode;
			DefaultTickrate = tickrate;
			Maps = maps;
		}
		public Game(string name, int tickrate, IEnumerable<string> names, IEnumerable<string> aliases)
		{
			Name = name;
			DefaultTickrate = tickrate;
			var maps = new List<Map>();
			for (int i = 0; i < names.ToList().Count; i++)
				maps.Add(new Map(names.ElementAt(i), aliases.ElementAt(i)));
			Maps = maps;
		}
		public Game(string name, string mode, int tickrate, IEnumerable<string> names, IEnumerable<string> aliases)
		{
			Name = name;
			Mode = mode;
			DefaultTickrate = tickrate;
			var maps = new List<Map>();
			for (int i = 0; i < names.ToList().Count; i++)
				maps.Add(new Map(names.ElementAt(i), aliases.ElementAt(i)));
			Maps = maps;
		}

		public bool HasMap(string mapName, bool ignoreCase = false)
			=> (ignoreCase) ? (bool)GetMapNames()?.Select(map => map.ToLower()).Contains(mapName.ToLower())
							: (bool)GetMapNames()?.Contains(mapName);
		public string GetMapAlias(string name)
			=> Maps?.FirstOrDefault(map => map.Name == name)?.Alias;

		public IEnumerable<string> GetMapNames()
			=> Maps?.Select(map => map.Name);
		public IEnumerable<string> GetMapAliases()
			=> Maps?.Select(map => map.Alias);

		public override string ToString()
			=> Name + (string.IsNullOrEmpty(Mode) ? string.Empty : $" ({Mode})");
	}
}