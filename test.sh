if [ "$1" = "CLI" ]; then
  dotnet run -p src/SourceDemoParser-CLI/ -c Release -f netcoreapp1.1 --no-build "messages=3;packets=3;commands=3;time;ticks;adj2;time;ticks" "demos/portal2_coop.dem"
else
  dotnet run -p src/SourceDemoParser.Net.Test/ -c ${1-PARSE_T} -f netcoreapp1.1 --no-build
fi