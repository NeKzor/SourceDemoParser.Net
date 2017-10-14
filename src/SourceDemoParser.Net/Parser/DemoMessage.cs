namespace SourceDemoParser
{
	public class DemoMessage : IDemoMessage
	{
		public DemoMessageType Type { get; set; }
		public int CurrentTick { get; set; }
		public byte? Tag { get; set; }
		public IFrame Frame { get; set; }

		public DemoMessage()
		{
		}
		public DemoMessage(DemoMessageType type, int tick, byte? tag, IFrame frame)
		{
			Type = type;
			CurrentTick = tick;
			Tag = tag;
			Frame = frame;
		}

		public override string ToString()
		{
			// Eh, wrong type for protocol version != 4
			return $"{Type}\t[{CurrentTick}:{Frame?.RawData?.Length ?? 0}]\t{(Frame?.ToString() ?? "NULL")}";
		}
	}
}