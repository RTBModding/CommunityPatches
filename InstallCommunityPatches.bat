@echo off

if not exist "RaceTrackBuilder.exe" (
    echo ERROR: RaceTrackBuilder.exe not found in the current folder.
    echo Please run this script from the folder containing RaceTrackBuilder.exe.
    echo.
    pause
    exit /b 1
)


powershell -NoProfile -Command "if (Get-Process -Name 'RaceTrackBuilder' -ErrorAction SilentlyContinue) { exit 0 } else { exit 1 }"
if %ERRORLEVEL%==0 (
  echo ERROR: RaceTrackBuilder.exe is currently running. Save your project, close RTB and try again.
  echo.
  pause
  exit /b 1
)

if not exist "Plugins\Height" mkdir "Plugins\Height"

if exist "Plugins\Height\hook.dll" (
    del /f /q "Plugins\Height\hook.dll"
    if exist "Plugins\Height\hook.dll" (
        echo ERROR: Failed to delete hook.dll
        echo Please manually delete 'Plugins\Height\hook.dll' and try again.
        echo.
        pause
        exit /b 5
    )
)

echo Downloading community patches...
powershell -NoProfile -Command "Invoke-WebRequest 'https://github.com/RTBModding/CommunityPatches/releases/latest/download/hook.dll' -OutFile 'Plugins\Height\hook.dll' -UseBasicParsing -ErrorAction Stop"

if not exist "Plugins\Height\hook.dll" (
    echo ERROR: Failed to download hook.dll
    echo.
    pause
    exit /b 2
)

powershell -NoProfile -Command ^
"Unblock-File 'Plugins\Height\hook.dll'; ^
 if (Get-Item -Path 'Plugins\Height\hook.dll' -Stream Zone.Identifier -ErrorAction SilentlyContinue) { exit 1 } else { exit 0 }"

if %ERRORLEVEL%==1 (
    echo ERROR: Failed to unblock hook.dll
    echo.
    pause
    exit /b 4
)

cls
echo hook.dll successfully installed. Launch RTB and the CommunityPatches are automatically loaded.
echo.
pause
