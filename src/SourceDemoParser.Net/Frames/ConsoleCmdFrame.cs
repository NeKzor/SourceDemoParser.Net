using System.Text;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public class ConsoleCmdFrame : IDemoFrame
    {
        public byte[] Data { get; set; }
        public string ConsoleCommand { get; set; }

        Task IDemoFrame.Parse(SourceDemo demo)
        {
            ConsoleCommand = Encoding.ASCII
                .GetString(Data)
                .TrimEnd(new char[1]);
            return Task.CompletedTask;
        }
        Task<byte[]> IDemoFrame.Export(SourceDemo demo)
        {
            var data = new byte[0];
            $"{ConsoleCommand}\0".ToBytes().AppendTo(ref data);
            return Task.FromResult(data);
        }
    }
}
