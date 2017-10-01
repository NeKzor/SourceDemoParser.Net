if [ "$1" == "DEBUG" ]
  then
  echo "TEST: CLI (DEBUG)"
  echo ""
  mono SourceDemoParser-CLI/bin/Debug/SourceDemoParser-CLI.exe $2 $3
  echo ""
else
  echo "TEST: CLI"
  echo ""
  mono SourceDemoParser-CLI/bin/Release/SourceDemoParser-CLI.exe $1 $2
  echo ""
fi