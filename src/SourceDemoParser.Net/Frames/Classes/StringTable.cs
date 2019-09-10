using System.Collections.Generic;
using System.Diagnostics;

namespace SourceDemoParser
{
    [DebuggerDisplay("{Name,nq} [{Entries.Count}:{ClientEntries.Count}]")]
    public class StringTable
    {
        public string Name { get; set; }
        public List<StringTableEntry> Entries { get; set; }
        public List<ClientEntry> ClientEntries { get; set; }

        public StringTable()
        {
            Entries = new List<StringTableEntry>();
            ClientEntries = new List<ClientEntry>();
        }
        public StringTable(string name) : this()
        {
            Name = name;
        }

        public void AddEntry(StringTableEntry entry)
            => Entries.Add(entry);
        public void AddClientEntry(ClientEntry entry)
             => ClientEntries.Add(entry);
    }

    [DebuggerDisplay("{Name,nq}")]
    public class StringTableEntry
    {
        public string Name { get; set; }
        public byte[] RawData { get; set; }
        public long Version { get; set; }
        public long Xuid { get; set; }
        public TableInfoBase Info { get; set; }
    }

    [DebuggerDisplay("{Name,nq}")]
    public class ClientEntry
    {
        public string Name { get; set; }
        public byte[] RawData { get; set; }
    }

    [DebuggerDisplay("{Name,nq}")]
    public class PlayerInfo : TableInfoBase
    {
        //public long Version { get; set; }
        //public long Xuid { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Guid { get; set; }
        public int FriendsId { get; set; }
        public string FriendsName { get; set; }
        public bool Fakeplayer { get; set; }
        public bool IsHltv { get; set; }
        public IEnumerable<int> CustomFiles { get; set; }
        public char FilesDownloaded { get; set; }
    }

    [DebuggerDisplay("{Id,nq}")]
    public class InstanceBaseline : TableInfoBase
    {
        public int Id { get; set; }
    }

    public abstract class TableInfoBase
    {
    }
}
