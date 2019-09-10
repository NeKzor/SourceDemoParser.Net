using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public abstract class SourceExporterBase
    {
        public ExportMode Mode { get; set; }

        protected SourceExporterBase(ExportMode mode)
            => Mode = mode;

        public abstract Task ExportAsync(BinaryWriter bw, SourceDemo demo);
        public abstract Task ExportHeader(BinaryWriter bw, SourceDemo demo);
    }
}
