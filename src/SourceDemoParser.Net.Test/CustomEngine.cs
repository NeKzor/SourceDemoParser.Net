using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser;

namespace CustomEngine
{
public class ExampleFrame : IDemoFrame
{
  public byte[] Data { get; set; }

  // Will be called if parsing mode is
  // set to "Everything"
  Task IDemoFrame.Parse(SourceDemo demo)
  {
    // Parse Data into something readable
    return Task.CompletedTask;
  }
  // For exporting edited data
  Task<byte[]> IDemoFrame.Export(SourceDemo demo)
  {
    // Reverse parsing logic here
    return Task.FromResult(Data);
  }
}

public class ExampleDemoMessage : DemoMessage
{
  public override Task Parse(BinaryReader br, SourceDemo demo)
  {
    Data = br.ReadBytes(br.ReadInt32());
    return Task.CompletedTask;
  }
  public override Task Export(BinaryWriter bw, SourceDemo demo)
  {
    bw.Write(Data.Length);
    bw.Write(Data);
    return Task.CompletedTask;
  }
}

public class Example : DemoMessageType<ExampleDemoMessage>
{
  public Example(int code) : base(code)
  {
  }
}

public static class ExampleDemoMessages
{
  // Demo message type will be handled by list index
  // Example: code = 0x03 => type = list[code - 1] = SyncTick
  public static List<DemoMessageType> ExampleEngine;

  static ExampleDemoMessages()
  {
    // Note: Message name "Stop" should always be the last
	// message of the demo
    ExampleEngine = DemoMessages.Default;
    // New message handled at 0x0A
    ExampleEngine.Add(new Example(0x0A));
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