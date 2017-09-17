namespace SourceDemoParser.Net.Extensions
{
	public interface ISourceDemo
	{
		string GameDirectory { get; }
		uint DefaultTickrate { get; }
	}
}