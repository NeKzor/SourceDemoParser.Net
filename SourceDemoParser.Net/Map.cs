namespace SourceDemoParser.Net
{
	public class Map
	{
		public string Name { get; }
		public string Alias { get; }

		public Map(string name)
			=> Name = name;
		public Map(string name, string alias)
		{
			Name = name;
			Alias = alias;
		}

		public override string ToString()
			=> Name + (string.IsNullOrEmpty(Alias) ? string.Empty : $" ({Alias})");
	}
}