using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
	public abstract class SourceParserBase : ISourceParser
	{
		public ParsingMode Mode { get; set; }
		public bool AutoAdjustment { get; set; }

		public static int MaxSplitscreenClients = 2;

		protected SourceParserBase(ParsingMode mode, bool autoAdjustment)
		{
			Mode = mode;
			AutoAdjustment = autoAdjustment;
		}

		public abstract Task<SourceDemo> ParseAsync(Stream input);
		public abstract Task<IFrame> HandleMessageAsync(BinaryReader br, IDemoMessage message);

		public virtual Task<IFrame> ParsePacketAsync(BinaryReader br)
		{
			var info = br.ReadBytes((MaxSplitscreenClients * 76) + 4 + 4);
			var length = br.ReadInt32();
			var net = br.ReadBytes(length);

			return Task.FromResult(new PacketFrame(info, net) as IFrame);
		}
		public virtual Task<IFrame> ParseSyncTickAsync(BinaryReader br)
			=> Task.FromResult(default(IFrame));
		public virtual Task<IFrame> ParseConsoleCmdAsync(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new ConsoleCmdFrame(data) as IFrame);
		}
		public virtual Task<IFrame> ParseUserCmdAsync(BinaryReader br)
		{
			var cmd = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new UserCmdFrame(cmd, data) as IFrame);
		}
		public virtual Task<IFrame> ParseDataTablesAsync(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new DataTablesFrame(data) as IFrame);
		}
		public virtual Task<IFrame> ParseStopAsync(BinaryReader br)
			=> Task.FromResult(default(IFrame));
		public virtual Task<IFrame> ParseCustomDataAsync(BinaryReader br)
		{
			var idk = br.ReadInt32();
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new CustomDataFrame(idk, data) as IFrame);
		}
		public virtual Task<IFrame> ParseStringTablesAsync(BinaryReader br)
		{
			var length = br.ReadInt32();
			var data = br.ReadBytes(length);

			return Task.FromResult(new StringTablesFrame(data) as IFrame);
		}
	}
}