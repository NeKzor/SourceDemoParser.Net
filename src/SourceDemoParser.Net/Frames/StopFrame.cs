using System.Threading.Tasks;

namespace SourceDemoParser
{
    public class StopFrame : IDemoFrame
    {
        public byte[] Data { get; set; }

        Task IDemoFrame.Parse(SourceDemo demo)
            => Task.CompletedTask;
        Task<byte[]> IDemoFrame.Export(SourceDemo demo)
            => Task.FromResult(default(byte[]));
    }
}
