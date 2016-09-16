cls
rem @echo off
rem ***************************************************************************
rem * Script to get the latest sources for MapWindow                          * 
rem * Created by: Paul Meems                                                  *
rem * 23 May 2011                                                             *
rem * Use TortoiseSVN to update your source code                              *
rem * Useful documentation:                                                   *
rem * http://tortoisesvn.net/docs/release/TortoiseSVN_en/tsvn-automation.html *
rem * http://ss64.com/nt/for_l.html                                           *
rem ***************************************************************************

rem location of the upper folder:
set _devFolder=C:\Dev

set _svn="C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe"
IF NOT EXIST %_svn% GOTO NoTortoise
set _logDir=%_devFolder%\MapWindow4Dev\WeeklyBuilds\Logs\svn

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir%

REM Clean-up logfiles:
del /q %_logDir%\*.*

FOR %%G IN (MapWindow4Dev MapWinGIS4Dev MapWindow4Plugins TestingScripts BASINS40 D4EM SwatObject) DO (
	IF EXIST %_devFolder%\%%G\ (
		echo [%DATE% %Time%] Start SVN update for %%G >%_logDir%\%%G.log
		%_svn% /command:update /path:"%_devFolder%\%%G\" /closeonend:2 
		echo [%DATE% %Time%] Finished SVN update for %%G >>%_logDir%\%%G.log
	)
)

REM Support libraries binaries and such:
set _source=SupportLibraries
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% /command:update /path:"%_devFolder%\%_source%\bin" /closeonend:2 
echo [%DATE% %Time%] Finished SVN update for %_source%\bin >>%_log%
%_svn% /command:update /path:"%_devFolder%\%_source%\lib" /closeonend:2
echo [%DATE% %Time%] Finished SVN update for %_source%\lib >>%_log%
%_svn% /command:update /path:"%_devFolder%\%_source%\include" /closeonend:2
echo [%DATE% %Time%] Finished SVN update for %_source%\include >>%_log%

REM open log files folder 
explorer %_logDir%

REM Open log browser of MapWinGIS:
%_svn% /command:log /path:"%_devFolder%\MapWinGIS4Dev\" /closeonend:0

goto:eof


:NoTortoise
echo TortoiseProc.exe isn't found at
echo %_svn%
echo Fix it before continuing
pause


