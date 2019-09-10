using System.Threading.Tasks;

namespace SourceDemoParser
{
    public interface INetMessage
    {
        NetMessageType Type { get; set; }
        Task Parse(SourceBufferReader buf, SourceDemo demo);
        Task Export(SourceBufferWriter bw, SourceDemo demo);
    }
}
