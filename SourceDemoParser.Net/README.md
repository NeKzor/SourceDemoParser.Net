# About
This was once a copy of a copy of a copy of a copy of a copy.

# How to use in c hashtag

#### 1.)
```cs
using SourceDemoParser.Net;
```

#### 2.a)
```cs
try
{
    // Async
    var demo = await SourceDemo.ParseFileAsync("path/just-a-demo-file.dem");
    // Sync
    demo = SourceDemo.ParseFileAsync("file-with-valid-contents.idk").GetAwaiter().GetResult();
}
catch
{
}
```

#### 2.b)
```cs
// Async
if (await SourceDemo.TryParseFile("rank2.dem", out var demo))
{
}
// Sync
if (SourceDemo.TryParseFile("almost-wr.dem", out demo).GetAwaiter().GetResult())
{
}
```

# Credits
Some other guy.