# Notes
* Parses `<command> <filepath>`
* Parses `<command1>;<command2>;<command3> <filepath>`
* Returns `Could not parse the demo file!` when demo parsing failed
* Returns `Could not parse any commands!` when command parsing failed
* Can return `<name>\t<data>`
* Can return `<name>\t<data>\n(...)<name>\t<data>`
* Can return `<name>\n<data>\t<data>\n(...)<data>\n<data>`
* Returns nothing for: `adj`, `adj2`, `adj-sf`
* Can parse int for: `adj=<endingtick>`
* Can parse int,int for: `adj=<startingtick>,<endingtick>`
* Can parse int for: `messages=<count>`, `commands=<count>`, `packets=<count>`
* Can parse string[] for: `commands=<ignore_command1>,<ignore_command2>,(...)>`