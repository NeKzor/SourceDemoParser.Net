using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public class DataTablesFrame : IFrame
	{
		public byte[] RawData { get; set; }
		public List<SendTable> Tables { get; set; }
		public List<ServerClassInfo> Classes { get; set; }

		public DataTablesFrame()
		{
			Tables = new List<SendTable>();
			Classes = new List<ServerClassInfo>();
		}
		public DataTablesFrame(byte[] data) : this()
		{
			RawData = data;
		}

		Task IFrame.ParseData(SourceDemo demo)
		{
			var buf = new BitBuffer(RawData);
			while (buf.ReadBoolean())
			{
				bool needsdecoder = buf.ReadBoolean();
				var table = new SendTable()
				{
					NetTableName = buf.ReadString(),
					NeedsDecoder = needsdecoder
				};
				var props = buf.ReadUBits(DataTable.PROPINFOBITS_NUMPROPS);
				for (int j = 0; j < props; j++)
				{
					const int protocol = 4;
					const int fbits = (protocol == 2) ? 11 : DataTable.PROPINFOBITS_FLAGS;

					var prop = new SendProp
					{
						Type = (SendPropType)buf.ReadUBits(DataTable.PROPINFOBITS_TYPE),
						VarName = buf.ReadString(),
						Flags = (SendPropFlags)buf.ReadUBits(fbits)
					};

					if ((prop.Type == SendPropType.DataTable) || (prop.IsExcludeProp()))
					{
						prop.ExcludeDtName = buf.ReadString();
						//if ((prop.Flags & SendPropFlags.Collapsible) != 0)
						//{
						//}
					}
					else if (prop.Type == SendPropType.Array)
					{
						prop.Elements = (int)buf.ReadUBits(DataTable.PROPINFOBITS_NUMELEMENTS);
					}
					else
					{
						prop.LowValue = buf.ReadSingle();
						prop.HighValue = buf.ReadSingle();
						prop.Bits = (int)buf.ReadUBits(DataTable.PROPINFOBITS_NUMBITS + 1);
					}
					table.Props.Add(prop);
				}
				Tables.Add(table);
			}

			var classes = buf.ReadInt16();
			for (var i = 0; i < classes; i++)
			{
				Classes.Add(new ServerClassInfo
				{
					ClassId = buf.ReadInt16(),
					ClassName = buf.ReadString(),
					DataTableName = buf.ReadString()
				});
			}
			return Task.CompletedTask;
		}
		Task<byte[]> IFrame.ExportData()
		{
			var data = new byte[0];
			// TODO
			return Task.FromResult(data);
		}
	}
}