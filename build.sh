dotnet build src/SourceDemoParser.Net/ -c Release -f netstandard1.4
dotnet build src/SourceDemoParser.Net.Test/ -c ${1-PARSE_T} -f netcoreapp1.1