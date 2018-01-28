using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SourceDemoParser;

namespace ReadmeExample
{
// This contains the data after reading
// the message tick or alignment byte
public class CustomFrame : IFrame
{
  public byte[] RawData { get; set; }

  public CustomFrame(byte[] data)
  {
    RawData = data;
  }

  // Will be called if parsing mode is
  // set to "Everything"
  Task IFrame.ParseData(SourceDemo demo)
  {
    // Parse RawData into something readable
    return Task.CompletedTask;
  }
  // For exporting edited data
  Task<byte[]> IFrame.ExportData()
  {
    // Reverse parsing logic here
    return Task.FromResult(RawData);
  }
}

public class CustomMessageParsers
{
  // This will be called after reading alignment byte or message tick
  public static Task<IFrame> ParseCustomMessage(BinaryReader br, SourceDemo demo)
  {
    var length = br.ReadInt32();
    var data = br.ReadBytes(length);
    return Task.FromResult(new CustomFrame(data) as IFrame);
  }
}

public class CustomMessageExporters
{
  public static Task ExportCustomMessage(BinaryWriter bw, IFrame frame)
  {
		bw.Write((frame as CustomFrame).RawData.Length);
    bw.Write((frame as CustomFrame).RawData);
    return Task.CompletedTask;
  }
}

public class CustomDemoMessages
{
  // Demo message type will be handled by list index
  // Example: code = 0x03 => type = list[code - 1] = SyncTick
  public static List<DemoMessageType> CustomEngine;

  static CustomDemoMessages()
  {
    CustomEngine = DemoMessages.Default;
    // New message handled at 0x0A
    CustomEngine.Add(new DemoMessageType(
      "MyMessage",
      CustomMessageParsers.ParseCustomMessage,
      CustomMessageExporters.ExportCustomMessage
    ));
  }
}

public class CustomParser : SourceParser
{
  // Detect your custom demo here
  public override Task Configure(SourceDemo demo)
  {
    _ = base.Configure(demo);

    switch (demo.GameDirectory)
    {
      case "custom_mod":
        // Overwrite default game messages with yours
        demo.Game.DefaultMessages = CustomDemoMessages.CustomEngine;
        break;
    }
    return Task.CompletedTask;
  }
}
}