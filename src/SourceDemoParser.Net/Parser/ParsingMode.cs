using System;

namespace SourceDemoParser
{
    [Flags]
    public enum ParsingMode
    {
        Default      = 0b_0000_0000,
        Header       = 0b_0000_0001,
        ConsoleCmd   = 0b_0000_0010,
        CustomData   = 0b_0000_0100,
        DataTables   = 0b_0000_1000,
        Packet       = 0b_0001_0000,
        StringTables = 0b_0010_0000,
        UserCmd      = 0b_0100_0000,
        Everything   = Header | ConsoleCmd | CustomData | DataTables | Packet | StringTables | UserCmd
    }
}
