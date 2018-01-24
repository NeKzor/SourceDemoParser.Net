namespace SourceDemoParser
{
	public class DemoMessage : IDemoMessage
	{
		public DemoMessageType Type { get; set; }
		public int CurrentTick { get; set; }
		public IFrame Frame { get; set; }

		public DemoMessage()
		{
		}
		public DemoMessage(DemoMessageType type, int tick)
		{
			Type = type;
			CurrentTick = tick;
		}

		public override string ToString()
		{
			return $"{Type}\t{CurrentTick}";
		}
	}
}