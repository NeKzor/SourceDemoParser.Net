using System.Threading.Tasks;
using SourceDemoParser.Messages;

namespace SourceDemoParser
{
    using DemoMessageType = System.Type;

    public abstract class DemoMessage : IDemoMessage
    {
        public byte Code { get; set; }
        public int Tick { get; set; }
        public byte? Slot { get; set; }
        public string Name { get; }

        public DemoMessage() => Name = GetType().Name;
        public DemoMessage(byte code) : this() => Code = code;

        public bool IsType<T>() => typeof(T) == GetType();

        public abstract Task Read(SourceBufferReader buf, SourceDemo demo);
        public abstract Task Write(SourceBufferWriter buf, SourceDemo demo);

        public override string ToString() => $"tick {Tick} 0x{Code.ToString("X")} as {Name}";
    }

    public static class DemoMessages
    {
        public static DemoMessageType[] Default => NewEngine;

        public static readonly DemoMessageType[] NewEngine = new DemoMessageType[]
        {
            default,
            typeof(SignOn),
            typeof(Packet),
            typeof(SyncTick),
            typeof(ConsoleCmd),
            typeof(UserCmd),
            typeof(DataTables),
            typeof(Stop),
            typeof(CustomData),
            typeof(StringTables)
        };

        public static readonly DemoMessageType[] OldEngine = new DemoMessageType[]
        {
            default,
            typeof(SignOn),
            typeof(Packet),
            typeof(SyncTick),
            typeof(ConsoleCmd),
            typeof(UserCmd),
            typeof(DataTables),
            typeof(Stop),
            typeof(StringTables)
        };
    }
}
