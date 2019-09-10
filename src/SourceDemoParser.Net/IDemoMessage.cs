using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public interface IDemoMessage
    {
        byte[] Data { get; set; }
        DemoMessageType Type { get; set; }
        int Tick { get; set; }
        byte? Slot { get; set; }
        IDemoFrame Frame { get; set; }
        Task Parse(BinaryReader br, SourceDemo demo);
        Task Export(BinaryWriter bw, SourceDemo demo);
    }
}
