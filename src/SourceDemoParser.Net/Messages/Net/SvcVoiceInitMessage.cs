using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcVoiceInitMessage : NetMessage
	{
		public string VoiceCodec { get; set; }
		public byte Quality { get; set; }

		public SvcVoiceInitMessage(NetMessageType type) : base(type)
		{
		}

		public override Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			VoiceCodec = buf.ReadString(); // MAX_OSPATH
			Quality = buf.ReadByte();
			System.Diagnostics.Debug.WriteLine("VoiceCodec: " + VoiceCodec);
			System.Diagnostics.Debug.WriteLine("Quality: " + Quality);
			/* if (Quality == (byte)255)
				_ = buf.ReadSingle(); */
			return Task.CompletedTask;
		}
		public override Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			bw.WriteString(VoiceCodec);
			bw.WriteByte(Quality);
			return Task.CompletedTask;
		}
	}
}