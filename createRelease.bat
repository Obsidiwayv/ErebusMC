@echo off

echo Copying mainstream launcher files to release...

set STATE=Debug
set LAUNCHERPATH=Launcher
set UPDATERPATH=Update

if not exist "%cd%\PublicRelease\%LAUNCHERPATH%\" mkdir %cd%\PublicRelease\%LAUNCHERPATH%\
if not exist "%cd%\PublicRelease\%UPDATERPATH%\" mkdir %cd%\PublicRelease\%UPDATERPATH%\

copy %cd%\ErebusLauncher\bin\x64\%STATE%\net6.0-windows10.0.17763.0 %cd%\PublicRelease\%LAUNCHERPATH%\

pause