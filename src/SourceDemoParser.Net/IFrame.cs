using System.Threading.Tasks;

namespace SourceDemoParser
{
    public interface IDemoFrame
    {
        byte[] Data { get; set; }
        Task Parse(SourceDemo demo);
        Task<byte[]> Export(SourceDemo demo);
    }
}
