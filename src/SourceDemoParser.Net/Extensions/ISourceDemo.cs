namespace SourceDemoParser.Extensions
{
	public interface ISourceDemo
	{
		string GameDirectory { get; }
		uint DefaultTickrate { get; }
	}
}