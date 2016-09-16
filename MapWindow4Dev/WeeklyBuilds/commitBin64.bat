@echo off
rem ************************************************
rem * Script to commit new binaries for MapWindow  *
rem * Created by: Paul Meems                       *
rem * 12 May 2009                                  *
rem * Copied on 1 Nov 2010 for x64 binaries        *
rem ************************************************

set _svn="C:\Program Files\SlikSvn\bin\svn.exe"\
IF NOT EXIST %_svn% set _svn="D:\Program Files\SlikSvn\bin\svn.exe"
set _log=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\svn\commit64.log
set _binFolder=C:\Dev\MapWindow4Dev\Bin64

echo [%DATE% %Time%] Start of commiting files >%_log%

echo [%DATE% %Time%] First clean-up >>%_log%
del /q /s %_binFolder%\*.pdb >>%_log%

cd %_binFolder%
rem first resolve all possible conflicts:
%_svn% resolve --depth=infinity --accept=mine-full %_binFolder% >>%_log%
%_svn% commit -m "Weekly build - 64Bit" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Committing binaries) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] 64Bit binaries committed >%_log%

echo [%DATE% %Time%] Finished commiting files >>%_log%

cd C:\Dev\MapWindow4Dev\WeeklyBuilds
%_svn% commit -m "Logfiles of the Weekly build - x64"
if not %errorlevel% == 0 echo     Error: %errorlevel% (Logfiles of the Weekly build - x64) >>%_log%
if %errorlevel% == 1 goto Err
echo [%DATE% %Time%] Finished commiting logfiles >>%_log%

echo [%DATE% %Time%] Committing BASINS x64 files >>%_log%
call commitBasins64.bat

goto:eof

:Err
echo [%DATE% %Time%] An error has occured. The job was not completed >>%_log%
echo %errorlevel%
