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
		public string Name { get; set; }						// Scoreboard information
		public int UserId { get; set; }							// Local server user ID, unique while server is running
		public string Guid { get; set; }						// Global unique player identifier
		public int FriendsId { get; set; }						// Friends identification number
		public string FriendsName { get; set; }					// Friends name
		public bool Fakeplayer { get; set; }					// True, if player is a bot controlled by game.dll
		public bool IsHltv { get; set; }						// True, if player is the HLTV proxy
		public IEnumerable<int> CustomFiles { get; set; }		// Custom files CRC for this player
		public char FilesDownloaded { get; set; }				// This counter increases each time the server downloaded a new file
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