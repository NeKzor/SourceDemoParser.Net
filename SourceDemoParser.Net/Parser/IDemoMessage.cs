namespace SourceDemoParser
{
	public interface IDemoMessage
	{
		DemoMessageType Type { get; set; }
		int CurrentTick { get; set; }
		byte? Tag { get; set; }
		IFrame Frame { get; set; }
	}
}