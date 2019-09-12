using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public class SourceExporter
    {
        public virtual async Task<byte[]> ExportAsync(SourceDemo demo)
        {
            var buf = new SourceBufferWriter();
            await demo.WriteHeader(buf).ConfigureAwait(false);
            await demo.WriteMessagesAsync(buf).ConfigureAwait(false);
            return buf.Data;
        }

        public virtual async Task ExportToFileAsync(string filePath, SourceDemo demo)
        {
            File.WriteAllBytes(filePath, await ExportAsync(demo).ConfigureAwait(false));
        }
    }
}
