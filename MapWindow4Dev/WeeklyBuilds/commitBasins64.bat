@echo off
rem ************************************************
rem * Script to commit new binaries for BASINS     *
rem * Created by: Paul Meems                       *
rem * 04 January 2010                              *
rem ************************************************

set _svn="C:\Program Files\SlikSvn\bin\svn.exe"\
IF NOT EXIST %_svn% set _svn="D:\Program Files\SlikSvn\bin\svn.exe"
set _log=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\svn\commit64.log
set _binFolder=C:\dev\BASINS40\Bin64

echo [%DATE% %Time%] Start of commiting BASINS files >>%_log%

IF NOT EXIST %_binFolder% ( 
echo [%DATE% %Time%] Binfolder for BASINS doesn't exists >>%_log%
goto Err
)

echo [%DATE% %Time%] First clean-up >>%_log%
del /q /s %_binFolder%\*.pdb >>%_log%

cd %_binFolder%
rem first resolve all possible conflicts:
%_svn% resolve --depth=infinity --accept=mine-full %_binFolder% >>%_log%
%_svn% commit -m "Weekly build BASINS - 64Bit" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Committing binaries) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] 64Bit binaries for BASINS committed >%_log%

echo [%DATE% %Time%] Finished commiting files >>%_log%

cd C:\Dev\MapWindow4Dev\WeeklyBuilds
%_svn% commit -m "Logfiles of the Weekly build - x64"
if not %errorlevel% == 0 echo     Error: %errorlevel% (Logfiles of the Weekly build - x64) >>%_log%
if %errorlevel% == 1 goto Err

echo [%DATE% %Time%] Finished commiting logfiles >>%_log%
goto :eof

:Err
echo [%DATE% %Time%] An error has occured. The job was not completed >>%_log%
echo %errorlevel%

