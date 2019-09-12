using System.Diagnostics;
using System.Threading.Tasks;

namespace SourceDemoParser.Tests
{
    internal static class Paths
    {
        public const string Demos = "demos/public/";
        public const string Test = "demos/private/";
        public const string Export = "export/";
        public const string Logs = "logs/";
    }

    internal static class Program
    {
        private static void Main() => new App().RunTests();
        public static void Await(this Task task) => task.GetAwaiter().GetResult();
    }

    internal class App
    {
        public Test Test { get; }
        public Game GameTest { get; }
        public Splicer SplicerTest { get; }

        public App()
        {
            Test = new Test();
            GameTest = new Game();
            SplicerTest = new Splicer();
        }

        public void RunTests()
        {
            ParsingTest();
            TimingTest();
            ModificationTest();
            DiscoverTest();
            DirectLoadTest();
            CleanupTest();
            NetTest();

            OldEngineTest();
            PortalTest();
            Portal2Test();

            SpliceTest();
            SpliceAnalysis();
        }

        [Conditional("PARSE")]
        public void ParsingTest()
            => Test.Parsing().Await();
        [Conditional("PARSE_T")]
        public void TimingTest()
            => Test.Timing().Await();
        [Conditional("EDIT")]
        public void ModificationTest()
            => Test.Modification().Await();
        [Conditional("DISCOVER"), Conditional("DISCOVER_2")]
        public void DiscoverTest()
            => Test.Discover().Await();
        [Conditional("DIRECT")]
        public void DirectLoadTest()
            => Test.DirectLoad().Await();
        [Conditional("CLEANUP")]
        public void CleanupTest()
            => Test.Cleanup().Await();
        [Conditional("NET")]
        public void NetTest()
            => Test.Net().Await();

        [Conditional("OE")]
        public void OldEngineTest()
            => GameTest.OldEngine().Await();
        [Conditional("PORTAL")]
        public void PortalTest()
            => GameTest.Portal().Await();
        [Conditional("PORTAL2")]
        public void Portal2Test()
            => GameTest.Portal2().Await();

        [Conditional("SPLICE")]
        public void SpliceTest()
            => SplicerTest.Splice().Await();
        [Conditional("SPLICE_A")]
        public void SpliceAnalysis()
            => SplicerTest.Analyze().Await();
    }
}
