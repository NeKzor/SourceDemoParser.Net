# CLI

## Usage

`SourceDemoParser-CLI.dll rank2.dem`

## Options

- `--parsing-mode 0-2`
- `--output <command1> <command2> <etc.>`

## Output Mode Format

- `<name>\t<data>`
- `<name>\t<data>\n(...)<name>\t<data>`

## Commands

| Name | Description |
| --- | --- |
| header | - |
| header-id | - |
| protocol | - |
| netproc<br>net-protocol | - |
| dir<br>game-dir | - |
| map<br>map-name | - |
| server<br>server-name | - |
| client<br>client-name | - |
| time | - |
| ticks | - |
| frames | - |
| signon<br>signonlength | - |
| tickrate | Returns the calculated tickrate value. |
| ipt<br>interval-per-tick | Returns the calculated interval\_per\_tick value. |
| adj<br>adjust | Adjusts demo by fixing invalid ticks. |
| adj-sf<br>adjust-sf | Adjusts demo by searching for a save flag. |
| adj2<br>adjust2 | Adjusts demo by speedrunning rules. |