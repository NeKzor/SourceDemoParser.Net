using System.Threading.Tasks;

namespace SourceDemoParser
{
    public interface IDemoMessage
    {
        byte Code { get; }
        int Tick { get; }
        byte? Slot { get; }
        string Name { get; }
        Task Read(SourceBufferReader buf, SourceDemo demo);
        Task Write(SourceBufferWriter buf, SourceDemo demo);
    }
}
