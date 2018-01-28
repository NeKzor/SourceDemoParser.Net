dotnet build src/SourceDemoParser.Net/ -c Release -f netstandard1.4
if [ "$1" = "CLI" ]; then
  dotnet build src/SourceDemoParser-CLI/ -c Release -f netcoreapp1.1
else
  dotnet build src/SourceDemoParser.Net.Test/ -c ${1-PARSE_T} -f netcoreapp1.1
fi