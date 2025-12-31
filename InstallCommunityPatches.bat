REM    _____ _______ ____  _____    _____  _____ _____ _    _ _______   _    _ ______ _____  ______ 
REM   / ____|__   __/ __ \|  __ \  |  __ \|_   _/ ____| |  | |__   __| | |  | |  ____|  __ \|  ____|
REM  | (___    | | | |  | | |__) | | |__) | | || |  __| |__| |  | |    | |__| | |__  | |__) | |__   
REM   \___ \   | | | |  | |  ___/  |  _  /  | || | |_ |  __  |  | |    |  __  |  __| |  _  /|  __|  
REM   ____) |  | | | |__| | |      | | \ \ _| || |__| | |  | |  | |    | |  | | |____| | \ \| |____ 
REM  |_____/   |_|  \____/|_|      |_|  \_\_____\_____|_|  |_|  |_|    |_|  |_|______|_|  \_\______|
REM
REM When you are seeing this text, you probably did not right-click the download link to
REM select "Save link as...". Please go back and do so.
                                                                                                
                                                                                                


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

if exist "Plugins\Height\CommunityPatches.dll" (
    del /f /q "Plugins\Height\CommunityPatches.dll"
    if exist "Plugins\Height\CommunityPatches.dll" (
        echo ERROR: Failed to delete CommunityPatches.dll
        echo Please manually delete 'Plugins\Height\CommunityPatches.dll' and try again.
        echo.
        pause
        exit /b 5
    )
)

echo Downloading community patches...
powershell -NoProfile -Command "Invoke-WebRequest 'https://github.com/RTBModding/CommunityPatches/releases/latest/download/CommunityPatches.dll' -OutFile 'Plugins\Height\CommunityPatches.dll' -UseBasicParsing -ErrorAction Stop"

if not exist "Plugins\Height\CommunityPatches.dll" (
    echo ERROR: Failed to download CommunityPatches.dll
    echo.
    pause
    exit /b 2
)

powershell -NoProfile -Command ^
"Unblock-File 'Plugins\Height\CommunityPatches.dll'; ^
 if (Get-Item -Path 'Plugins\Height\CommunityPatches.dll' -Stream Zone.Identifier -ErrorAction SilentlyContinue) { exit 1 } else { exit 0 }"

if %ERRORLEVEL%==1 (
    echo ERROR: Failed to unblock CommunityPatches.dll
    echo.
    pause
    exit /b 4
)

cls
echo CommunityPatches.dll successfully installed. Launch RTB and the CommunityPatches are automatically loaded.
echo.
pause
