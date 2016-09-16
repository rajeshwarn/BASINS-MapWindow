rem ***************************************************
rem * Script to get the latest sources for MapWindow  *
rem * Created by: Paul Meems                          *
rem * 01 August 2010                                  *
rem ***************************************************

set _svn="C:\Program Files\SlikSvn\bin\svn.exe"
set _logDir=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\svn

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir%

REM set mwSupportLibraries as an environment vars 
REM if you have the source code in different locations:
if "%mwSupportLibraries%"=="" set mwSupportLibraries=C:\Dev\SupportLibraries

set _source=SharpDevelop
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

set _source=IndexSearching
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

set _source=WeifenLuoDockPanel
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

set _source=cqlib
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

REM log4net-1.2.10.dll already comes with SharpDevelop so it isn't
REM compiled seperately anymore
set _source=log4net-1.2.10
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

set _source=GDAL
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\%_source%" >>%_log%

