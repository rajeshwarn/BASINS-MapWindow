@echo off
rem ************************************************
rem * Script to commit new binaries for MapWindow  *
rem * Created by: Paul Meems                       *
rem * 12 May 2009                                  *
rem ************************************************

set _svn="C:\Program Files\SlikSvn\bin\svn.exe"
IF NOT EXIST %_svn% set _svn="D:\Program Files\SlikSvn\bin\svn.exe"
set _log=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\svn\commit.log

echo [%DATE% %Time%] Start of commiting files >%_log%

echo [%DATE% %Time%] First clean-up >>%_log%
del /q /s C:\Dev\MapWindow4Dev\Bin\*.pdb >>%_log%

cd C:\Dev\MapWindow4Dev\Bin
rem first resolve all possible conflicts:
%_svn% resolve --depth=infinity --accept=mine-full C:\Dev\MapWindow4Dev\Bin >>%_log%
%_svn% commit -m "Weekly build" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Committing binaries) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Binaries committed >%_log%

if "%mwSupportLibraries%"=="" set mwSupportLibraries=C:\Dev\SupportLibraries
cd %mwSupportLibraries%\bin
rem first resolve all possible conflicts:
%_svn% resolve --depth=infinity --accept=mine-full %mwSupportLibraries%\bin >>%_log%
%_svn% commit -m "Support libraries binaries" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Support libraries binaries) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Support libraries binaries committed >%_log%

cd %mwSupportLibraries%\lib
%_svn% commit -m "Support libraries libs" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Support libraries libs) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Support libraries libs committed >%_log%

cd %mwSupportLibraries%\include
%_svn% commit -m "Support libraries includes" >>%_log%
if not %errorlevel% == 0 echo     Error: %errorlevel% (Support libraries includes) >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Support libraries includes committed >%_log%

echo [%DATE% %Time%] Finished commiting files >>%_log%

cd C:\Dev\MapWindow4Dev\WeeklyBuilds
%_svn% commit -m "Logfiles of the Weekly build"
if not %errorlevel% == 0 echo     Error: %errorlevel% (Logfiles of the Weekly build) >>%_log%
if %errorlevel% == 1 goto Err
echo [%DATE% %Time%] Finished commiting logfiles >>%_log%
goto:eof

:Err
echo [%DATE% %Time%] An error has occured. The job was not completed >>%_log%
echo %errorlevel%
