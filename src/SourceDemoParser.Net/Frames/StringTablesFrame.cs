using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SourceDemoParser.Extensions;

namespace SourceDemoParser
{
	public class StringTablesFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public List<StringTable> Tables { get; set; }

		public StringTablesFrame()
		{
			Tables = new List<StringTable>();
		}
		public StringTablesFrame(byte[] data) : this()
		{
			RawData = data;
		}

		Task IFrame.ParseData(SourceDemo demo)
		{
			var buf = new BitBuffer(RawData);
			int tables = buf.ReadByte();
			for (int i = 0; i < tables; i++)
			{
				var name = buf.ReadString();
				var table = new StringTable(name);

				var entries = buf.ReadInt16();
				for (int j = 0; j < entries; j++)
				{
					var entry = buf.ReadString();
					var data = default(byte[]);
					var info = default(TableInfoBase);

					if (buf.ReadBoolean())
					{
						var length = buf.ReadInt16();
						data = buf.ReadBytes(length);

						// TODO
						if (name == Const.INSTANCE_BASELINE_TABLENAME)
						{
							info = new InstanceBaseline
							{
								Id = int.Parse(entry)
							};
						}
						else if (name == Const.LIGHT_STYLES_TABLENAME)
						{

						}
						else if (name == Const.SERVER_STARTUP_DATA_TABLENAME)
						{

						}
						else if (name == Const.USER_INFO_TABLENAME)
						{
							var buf2 = new BitBuffer(data);
							if (demo.GameDirectory == "csgo") // Hack
							{
								// 16 bytes = ???
								var temp1 = buf2.ReadBytes(8);
								var temp2 = buf2.ReadBytes(8);
								var version = BitConverter.ToInt64(temp1, 0);
								var xuid = BitConverter.ToInt64(temp2, 0);
							}
							else
							{
								// 8 bytes = ???
								var idk1 = buf2.ReadInt32();
								var idk2 = buf2.ReadInt32();
							}

							info = new PlayerInfo()
							{
								// 32 bytes
								Name = Encoding.ASCII.GetString(buf2.ReadBytes(Const.MAX_PLAYER_NAME_LENGTH)).TrimEnd('\0'),
								// 4 bytes
								UserId = buf2.ReadInt32(),
								// 33 bytes
								Guid = Encoding.ASCII.GetString(buf2.ReadBytes(Const.SIGNED_GUID_LEN + 1)).TrimEnd('\0'),
								// 4 bytes
								FriendsId = buf2.ReadInt32(),
								// 32 bytes
								FriendsName = Encoding.ASCII.GetString(buf2.ReadBytes(Const.MAX_PLAYER_NAME_LENGTH)).TrimEnd('\0'),
								// 1 byte
								Fakeplayer = buf2.ReadBoolean(),
								// 1 byte
								IsHltv = buf2.ReadBoolean(),
								// 16 bytes
								CustomFiles = new int[4]
								{
									buf2.ReadInt32(),
									buf2.ReadInt32(),
									buf2.ReadInt32(),
									buf2.ReadInt32()
								},
								// 2 bytes
								FilesDownloaded = buf2.ReadChar()
							};
						}
					}
					table.AddEntry(new StringTableEntry
					{
						Name = entry,
						RawData = data,
						Info = info
					});
				}

				if (buf.ReadBoolean())
				{
					var centries = buf.ReadInt16();
					for (var j = 0; j < centries; j++)
					{
						var centry = buf.ReadString();
						var data = default(byte[]);
						if (buf.ReadBoolean())
						{
							var length = buf.ReadInt16();
							data = buf.ReadBytes(length);
						}
						table.AddClientEntry(new ClientEntry
						{
							Name = centry,
							RawData = data
						});
					}
				}
				Tables.Add(table);
			}
			return Task.FromResult(false);
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			// TODO
			return Task.FromResult(data);
		}
	}
}