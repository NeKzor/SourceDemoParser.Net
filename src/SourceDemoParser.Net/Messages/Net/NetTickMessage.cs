using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class NetTickMessage : NetMessage
	{
		public int Tick { get; set; }
		public short HostFrameTime { get; set; }
		public short HostFrameTimeStdDeviation { get; set; }

		public NetTickMessage(NetMessageType type) : base(type)
		{
		}
		
		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			Tick = buf.ReadInt32();
			HostFrameTime = buf.ReadInt16();
			HostFrameTimeStdDeviation = buf.ReadInt16();
			System.Diagnostics.Debug.WriteLine(Tick);
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteInt32(Tick);
			bw.WriteInt16(HostFrameTime);
			bw.WriteInt16(HostFrameTimeStdDeviation);
			return Task.CompletedTask;
		}
	}
}