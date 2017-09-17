if [ "$1" == "DEBUG" ]
  then
    echo "BUILDING: DEBUG"
    echo ""
    xbuild /p:Configuration=Debug SourceDemoParser.Net.sln

else
  echo "BUILDING: RELEASE"
  echo ""
  xbuild /p:Configuration=Release SourceDemoParser.Net.sln
fi