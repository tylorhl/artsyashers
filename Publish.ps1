$workingDir = $PWD
cd "$PSScriptRoot/src/Tylorhl.ArtsyAshers";
dotnet publish -c Release;

rm "$PSScriptRoot/site/*" -r

cp ./bin/Release/netstandard2.0/publish/Tylorhl.ArtsyAshers/dist/* "$PSScriptRoot/site/" -Recurse

rm ./bin/Release -r

cd $workingDir