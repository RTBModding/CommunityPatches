@echo off
echo BUILD
dotnet build

REM --- Set paths ---
set SOURCE_DLL=bin\Debug\net48\CommunityPatches.dll
set HARMONY_DLL=hooklibs\0Harmony.dll

set MERGED_DLL=bin\Debug\net48\CommunityPatches_merged.dll
set DEST_DLL=..\Height\CommunityPatches.dll
set TARGET_APP=..\..\RaceTrackBuilder.exe

echo MERGE
"C:\Users\Sllux\.nuget\packages\ilmerge\3.0.41\tools\net452\ILMerge.exe" ^
  /target:library ^
  /internalize ^
  /out:%MERGED_DLL% ^
  %SOURCE_DLL% %HARMONY_DLL%

echo COPY
copy /Y "%MERGED_DLL%" "%DEST_DLL%"

echo EXECUTE
start "" "%TARGET_APP%"
