namespace SourceDemoParser
{
	public class DemoMessage : IDemoMessage
	{
		public DemoMessageType Type { get; set; }
		public int CurrentTick { get; set; }
		public IFrame Frame { get; set; }

		public override string ToString()
			=> $"{Type}\t{CurrentTick}";
	}
}