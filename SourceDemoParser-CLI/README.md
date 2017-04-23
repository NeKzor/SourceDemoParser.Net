# Notes
* Parses `<command> <filepath>`
* Parses `<command1>;<command2>;<command3> <filepath>`
* Returns `Could not parse the demo file!` when demo parsing failed
* Returns `Could not parse any commands!` when command parsing failed
* Can return `<name>\t<data>`
* Can return `<name>\t<data>\n(...)<name>\t<data>`
* Can return `<name>\n<data>\t<data>\n(...)<data>\n<data>`