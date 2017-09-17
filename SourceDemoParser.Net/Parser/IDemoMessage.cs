namespace SourceDemoParser.Net
{
	public interface IDemoMessage
	{
		DemoMessageType Type { get; set; }
		int CurrentTick { get; set; }
		byte? Optional { get; set; }
		IFrame Frame { get; set; }
	}
}