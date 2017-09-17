# How to use in c hashtag aka docs

- [Parsing](#parsing)
- [Events](#events)
- [Extensions](#extensions)
- [Adjustments](#adjustments)
- [Custom Adjustments](#custom-adjustments)
- [Parse, Edit & Export](#parse-edit--export)
- [Data Simulation](#data-simulation)

# Parsing
```cs
using SourceDemoParser.Net;

var parser = new SourceParser(headerOnly: true);
var demo = await parser.ParseFileAsync("rank2.dem");
```

# Events
```cs
using SourceDemoParser.Net;

Task Log_onMessage(object sender, DemoMessage msg)
{
	// Note that any long running code here can affect parsing time
	// Use async if needed
	Console.WriteLine(msg.ToString());
	return Task.FromResult(true);
}

// Somewhere else:
var parser = new SourceParser();
parser.OnDemoMessage += Log_onMessage;
//parser.OnDemoMessage += async (s, m) => Log_onMessage(s, m);
await parser.ParseFileAsync("new_route.dem");
```

# Extensions
```cs
using SourceDemoParser.Net.Extensions;

var tickrate = demo.GetTickrate();
var tps = demo.GetTicksPerSecond();
```

# Adjustments
```cs
using SourceDemoParser.Net.Extensions;

// Some games and mods (Portal 2 etc.) have issues when ending a demo
// through a changelevel. To fix the incorrect header (PlaybackTime and
// PlaybackTicks) we take the last positive tick of the parsed messages
await demo.AdjustExact();

// Adjustments for specific maps with special rules
await demo.AdjustAsync();

// Adjusts demo until a special command. Default standard is from
// the SourceRuns community (echo #SAVE#)
await demo.AdjustFlagAsync(saveFlag: "echo #IDEEDIT#");
```

# Custom Adjustments
```cs
using SourceDemoParser.Net.Extensions;

// Implement ISourceDemo
public class Portal2CustomMapDemo : ISourceDemo
{
	// Set demo folder and tickrate
	public string GameDirectory => "portal2";
	public uint DefaultTickrate => 60u;
	
	// Return boolean for an adjustment
	// Example: Find start tick of a specific map
	[StartAdjustment("sp_gud_mape")]
	public bool SpGudMape_Start(PlayerPosition pos)
	{
		// Search logic with: PlayerPosition
		var destination = new Vector3f(-723.00f, -2481.00f, 17.00f);
		if (Vector3f.Equals(pos.Old, destination))
			if (!(Vector3f.Equals(pos.Current, destination)))
				return true;
		return false;
	}
	
	// Example: Find end tick of a specific map with negative tick offset
	[EndAdjustment("sp_gud_mape_finale", -1)]
	public bool GgStageTheend_Ending(PlayerCommand cmd)
	{
		// Search logic with: PlayerCommand
		var command = "playvideo_exitcommand_nointerrupt at_credits end_movie credits_video";
		return (cmd.Current == command);
	}
	
	// Example: Find end tick of any map with positive tick offset
	[EndAdjustment(offset: 1)]
	public bool ForSpecialCasesAlwaysCheck_Ending(PlayerCommand cmd)
	{
		var command = "echo SPECIAL_FADEOUT_WITH_VALUE";
		return (cmd.Current.StartsWitch(command));
	}
}

// Somewhere else:
await demo.AdjustAsync<Portal2CustomMapDemo>();

// Stuff will be cached automatically afterwards
// Or search and load all demos with Reflection:
await demo.AdjustAsync(System.Reflection.Assembly.GetCurrentAssembly());

// Optionally use this alone if everything got cached anyway
await demo.AdjustAsync();
```
##### [More Demo Examples](https://github.com/NeKzor/SourceDemoParser.Net/tree/dev/SourceDemoParser.Net/Extensions/Demos)

# Parse, Edit & Export
```cs
using SourceDemoParser.Net;
using SourceDemoParser.Net.Extensions;

var parser = new SourceParser();
var demo = await parser.ParseFileAsync("just_a_wr_by_zypeh.dem");
demo.ClientName = "NeKz";
await demo.ExportFileAsync("zypeh_copied_me_hahaha.dem");
```

# Data Simulation
```cs
// Todo
```