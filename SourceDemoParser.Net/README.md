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
    var demo = await SourceDemo.Parse("path/just-a-demo-file.dem");
    // Sync
    demo = SourceDemo.Parse("file-with-valid-contents.idk").GetAwaiter().GetResult();
}
catch
{
}
```

#### 2.b)
```cs
// Async
if (await SourceDemo.TryParse("rank2.dem", out var demo))
{
}
// Sync
if (SourceDemo.TryParse("almost-wr.dem", out demo).GetAwaiter().GetResult())
{
}
```

# Credits
Some other guy.