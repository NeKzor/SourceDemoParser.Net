[![Build Status](https://travis-ci.org/NeKzor/SourceDemoParser.Net.svg?branch=dev)](https://travis-ci.org/NeKzor/SourceDemoParser.Net)
[![Build Version](https://img.shields.io/badge/version-v1.0-yellow.svg)](https://github.com/NeKzor/SourceDemoParser.Net/projects/1)
[![Release Status](https://img.shields.io/github/release/NeKzor/SourceDemoParser.Net.svg)](https://github.com/NeKzor/SourceDemoParser.Net/releases)

# SourceDemoParser.Net
Parse any demo with protocol version 2, 3 or 4.

Main features:
- Parse demo header
- Parse data beyond the header
- Fix negative time in demo header (called adjustment)
- Adjustment for special demo rules, defined by speedrunning communities

# Overview
|Namespace|Status|Description|
|---|:-:|---|
|[SourceDemoParser.Net](SourceDemoParser.Net)|✖|SourceDemo, SourceParser etc.|
|[SourceDemoParser.Net.Extensions](SourceDemoParser.Net/Extensions)|✖|Adjustment, exporting etc.|
|[SourceDemoParser.Net.Extensions.Demos](SourceDemoParser.Net/Extensions/Demos)|✖|Supported, default adjustments.|

# [C# Docs](DOCS.md)

# [CLI Tool](SourceDemoParser-CLI)
Simple tool for command line interfaces.

Example: `SourceDemoParser-CLI.exe header segment_42.dem`.