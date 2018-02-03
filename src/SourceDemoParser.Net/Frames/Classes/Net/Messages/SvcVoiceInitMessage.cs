namespace SourceDemoParser
{
	public class SvcVoiceInitMessage : INetMessage
	{
		public string VoiceCodec { get; set; }
		public byte Quality { get; set; }
	}
}