namespace SourceDemoParser.Net
{
	public class DemoMessage : IDemoMessage
	{
		public DemoMessageType Type { get; set; }
		public int CurrentTick { get; set; }
		public byte? Optional { get; set; }
		public IFrame Frame { get; set; }
		
		public DemoMessage()
		{
		}
		public DemoMessage(DemoMessageType type, int tick, byte? optional, IFrame frame)
		{
			Type = type;
			CurrentTick = tick;
			Optional = optional;
			Frame = frame;
		}
		
		public override string ToString()
		{
			return $"{Type}\t[{CurrentTick}{((Optional != null) ? $":{(int)Optional}" : string.Empty)}]\t{(Frame?.ToString() ?? "NULL")}";
		}
	}
}