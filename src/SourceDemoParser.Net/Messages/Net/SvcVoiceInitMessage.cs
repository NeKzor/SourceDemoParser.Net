using System.Threading.Tasks;

namespace SourceDemoParser.Messages.Net
{
	public class SvcVoiceInitMessage : INetMessage
	{
		public string VoiceCodec { get; set; }
		public byte Quality { get; set; }

		public Task Parse(ISourceBufferUtil buf, SourceDemo demo)
		{
			var codec = buf.ReadString(); // MAX_OSPATH
			var quality = buf.ReadByte();
			Debug.WriteLine("codec: " + codec);
			Debug.WriteLine("quality: " + quality);
			/* if (quality == (byte)255)
				_ = buf.ReadSingle(); */
			return Task.CompletedTask;
		}
		public Task Export(ISourceWriterUtil bw, SourceDemo demo)
		{
			return Task.CompletedTask;
		}
	}
}