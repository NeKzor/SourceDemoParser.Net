using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser;

namespace CustomEngine
{
public class ExampleFrame : IDemoFrame
{
  public byte[] RawData { get; set; }

  public ExampleFrame(byte[] data)
  {
    RawData = data;
  }

  // Will be called if parsing mode is
  // set to "Everything"
  Task IDemoFrame.Parse(SourceDemo demo)
  {
    // Parse RawData into something readable
    return Task.CompletedTask;
  }
  // For exporting edited data
  Task<byte[]> IDemoFrame.Export()
  {
    // Reverse parsing logic here
    return Task.FromResult(RawData);
  }
}

public class ExampleDemoMessage : DemoMessage
{
	public override Task<IDemoFrame> Parse(BinaryReader br, SourceDemo demo)
    {
      var length = br.ReadInt32();
      var data = br.ReadBytes(length);
      return Task.FromResult(Frame = new ExampleFrame(data) as IDemoFrame);
    }
    public override Task Export(BinaryWriter bw, SourceDemo demo)
    {
      bw.Write((Frame as ExampleFrame).RawData.Length);
      bw.Write((Frame as ExampleFrame).RawData);
      return Task.CompletedTask;
    }
}

public class Example : DemoMessageType
{
  public override IDemoMessage GetMessage()
    => new ExampleDemoMessage();
}

public static class ExampleDemoMessages
{
  // Demo message type will be handled by list index
  // Example: code = 0x03 => type = list[code - 1] = SyncTick
  public static List<DemoMessageType> ExampleEngine;

  static ExampleDemoMessages()
  {
    // Note: 0x07 is always "stop" for the parser
    ExampleEngine = DemoMessages.Default;
    // New message handled at 0x0A
    ExampleEngine.Add(new Example());
  }
}

public class ExampleParser : SourceParser
{
  // Detect your custom demo here
  public override Task Configure(SourceDemo demo)
  {
    _ = base.Configure(demo);

    switch (demo.GameDirectory)
    {
      case "example_mod":
        // Overwrite default game messages with yours
        demo.Game.DefaultMessages = ExampleDemoMessages.ExampleEngine;
        break;
    }
    return Task.CompletedTask;
  }
}
}