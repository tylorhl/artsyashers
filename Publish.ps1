$workingDir = $PWD
cd "$PSScriptRoot/src/Tylorhl.ArtsyAshers";
dotnet publish -c Release;

rm "$PSScriptRoot/docs/*" -r

cp ./bin/Release/netstandard2.0/publish/Tylorhl.ArtsyAshers/dist/* "$PSScriptRoot/docs/" -Recurse

echo $null >> "$PSScriptRoot/docs/.nojekyll"

rm ./bin/Release -r

cd $workingDir