namespace SourceDemoParser.Speedrun
{
    public interface ISourceDemo
    {
        string GameDirectory { get; }
        uint DefaultTickrate { get; }
    }
}
