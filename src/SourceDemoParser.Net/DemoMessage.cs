using System.IO;
using System.Threading.Tasks;

namespace SourceDemoParser
{
    public abstract class DemoMessage : IDemoMessage
    {
        public byte[] Data
        {
            get => Frame.Data;
            set => Frame.Data = value;
        }

        public DemoMessageType Type { get; set; }
        public int Tick { get; set; }
        public byte? Slot { get; set; }
        public IDemoFrame Frame { get; set; }

        public DemoMessage()
        {
        }
        public DemoMessage(DemoMessageType type)
        {
            Type = type;
        }

        public abstract Task Parse(BinaryReader br, SourceDemo demo);
        public abstract Task Export(BinaryWriter bw, SourceDemo demo);

        public override string ToString()
            => $"[{Tick}] 0x{Type.MessageType.ToString("X")} as {Type}";
    }
    public abstract class DemoMessage<T> : DemoMessage
        where T : IDemoFrame, new()
    {
        public DemoMessage()
        {
            Frame = new T();
        }
    }
}
