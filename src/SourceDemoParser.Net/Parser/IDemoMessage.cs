namespace SourceDemoParser
{
	public interface IDemoMessage
	{
		DemoMessageType Type { get; set; }
		int CurrentTick { get; set; }
		byte? Tag { get; set; }				// demo.Protocol == 4
		IFrame Frame { get; set; }
	}
}