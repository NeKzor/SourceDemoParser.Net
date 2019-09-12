using System.Threading.Tasks;

namespace SourceDemoParser
{
    public interface INetMessage
    {
        byte Code { get; }
        string Name { get; }
        bool IsType<T>();
        Task Read(SourceBufferReader buf, SourceDemo demo);
        Task Write(SourceBufferWriter buf, SourceDemo demo);
    }
}
