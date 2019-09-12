using System;
using System.Linq;
using System.Threading.Tasks;
using SourceDemoParser.Messages;

namespace SourceDemoParser.Tests
{
    internal class Game
    {
        public async Task OldEngine()
        {
            const string source = "hl2oe.dem";
            var parser = new SourceParser();
            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
            Console.WriteLine($"Messages: {demo.Messages.Count}");
        }
        public async Task Portal()
        {
            const string source = "portal_steampipe.dem";
            var parser = new SourceParser();
            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
            Console.WriteLine($"Messages: {demo.Messages.Count}");
            foreach (var message in demo.Messages.Take(50))
            {
                Console.WriteLine(message);
            }
        }
        public async Task Portal2()
        {
            const string source = "portal2_sp.dem";
            var parser = new SourceParser();
            var demo = await parser.ParseFromFileAsync(Paths.Demos + source);
            foreach (var message in demo.Messages)
            {
                if (message is UserCmd ucmsg)
                {
                    Console.WriteLine(message.Tick + ": " + (ucmsg as UserCmd).Cmd.SideMove);
                }
            }
        }
    }
}
