namespace SourceDemoParser.Net.Results
{
	public class ConsoleCommandFrame : IFrame
	{
		public string ConsoleCommand { get; set; }
		public int CurrentTick { get; set; }

		public ConsoleCommandFrame()
		{
		}
		public ConsoleCommandFrame(int tick, string command)
		{
			CurrentTick = tick;
			ConsoleCommand = command;
		}

		public override string ToString()
			=> $"{CurrentTick}\t{ConsoleCommand}";
	}
}