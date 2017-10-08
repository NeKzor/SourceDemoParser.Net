using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	[DebuggerDisplay("{Name,nq}")]
	public class StringTable
	{
		public string Name { get; set; }
		public List<StringTableEntry> Entries { get; set; }

		public StringTable()
		{
		}
		public StringTable(string name, int entryCount)
		{
			Name = name;
			Entries = new List<StringTableEntry>(entryCount);
		}

		public void AddEntry(StringTableEntry entry)
			=> Entries.Add(entry);
	}

	[DebuggerDisplay("{Name,nq}")]
	public class StringTableEntry
	{
		public string Name { get; set; }
		public byte[] RawData { get; set; }
	}

	public class StringTablesFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public List<StringTable> Tables { get; set; }

		public StringTablesFrame()
		{
			Tables = new List<StringTable>();
		}
		public StringTablesFrame(byte[] data)
		{
			RawData = data;
			Tables = new List<StringTable>();
		}

		Task IFrame.ParseData()
		{
			var buf = new BitBuffer(RawData);
			int tables = buf.ReadByte();
			for (int i = 0; i < tables; i++)
			{
				var name = buf.ReadString();
				var entries = buf.ReadInt16();
				var table = new StringTable(name, entries);

				for (int j = 0; j < entries; j++)
				{
					var entry = buf.ReadString();

					var data = default(byte[]);
					if (buf.ReadBoolean())
					{
						var length = buf.ReadInt16();
						data = buf.ReadBytes(length);
						if (entry == "userinfo")
							Debug.WriteLine("UserInfo");
					}

					table.AddEntry(new StringTableEntry
					{
						Name = entry,
						RawData = data
					});
				}
				Tables.Add(table);
			}
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			if (RawData == null)
				return Task.FromResult(default(byte[]));

			var bytes = RawData.GetBytes();
			return Task.FromResult(bytes);
		}

		public override string ToString()
			=> $"{Tables.Count}";
	}
}