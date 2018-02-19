if [ "$1" = "CLI" ]; then
  shift
  dotnet run -p src/SourceDemoParser-CLI/ -c Release -f netcoreapp1.1 --no-build -- "$@"
else
  dotnet run -p src/SourceDemoParser.Net.Test/ -c ${1-PARSE_T} -f netcoreapp1.1 --no-build
fi