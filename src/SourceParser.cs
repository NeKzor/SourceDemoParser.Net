using System;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;
using SourceDemoParser.Messages;

namespace SourceDemoParser
{
    public class SourceParserOptions
    {
        public bool ReadHeader { get; set; } = true;
        public bool ReadMessages { get; set; } = true;
        public bool ReadStringTables { get; set; } = false;
        public bool ReadDataTables { get; set; } = false;
        public bool ReadPackets { get; set; } = false;
        public bool ReadUserCmds { get; set; } = false;

        public static readonly SourceParserOptions Default = new SourceParserOptions();
    }

    public class SourceParser
    {
        public Func<SourceDemo, SourceGame> GameBuilder { get; set; }
        public SourceParserOptions Options { get; set; }

        public SourceParser(SourceParserOptions options = default, Func<SourceDemo, SourceGame> gameBuilder = default)
        {
            Options = options ?? SourceParserOptions.Default;
            GameBuilder = gameBuilder ?? SourceGameBuilder.Default;
        }

        public virtual async Task<SourceDemo> ParseAsync(byte[] content)
        {
            var demo = new SourceDemo();
            var buf = new SourceBufferReader(content);

            if (Options.ReadHeader)
                await demo.ReadHeader(buf).ConfigureAwait(false);

            if (Options.ReadMessages)
            {
                await Task.Run<SourceGame>(() => demo.Game = GameBuilder.Invoke(demo)).ConfigureAwait(false);

                if (!Options.ReadHeader)
                    buf.SeekBytes(1072);

                await demo.ReadMessagesAsync(buf).ConfigureAwait(false);

                foreach (var message in demo.Messages)
                {
                    if (Options.ReadPackets && message is Packet pa)
                    {
                        await demo.Read(pa).ConfigureAwait(false);
                        continue;
                    }
                    if (Options.ReadStringTables && message is StringTables st)
                    {
                        await demo.Read(st).ConfigureAwait(false);
                        continue;
                    }
                    if (Options.ReadDataTables && message is DataTables dt)
                    {
                        await demo.Read(dt).ConfigureAwait(false);
                        continue;
                    }
                    if (Options.ReadUserCmds && message is UserCmd uc)
                    {
                        await demo.Read(uc).ConfigureAwait(false);
                        continue;
                    }
                }
            }

            return demo;
        }

        public virtual async Task<SourceDemo> ParseFromFileAsync(string filePath)
        {
            return await ParseAsync(File.ReadAllBytes(filePath)).ConfigureAwait(false);
        }
    }
}
