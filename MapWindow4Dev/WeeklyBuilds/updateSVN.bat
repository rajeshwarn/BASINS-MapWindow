rem @echo off
rem ***************************************************
rem * Script to get the latest sources for MapWindow  *
rem * Created by: Paul Meems                          *
rem * 12 May 2009                                     *
rem * PM 24 Oct 2010: Made more generic and added     *
rem * more folders to update (if they exist)          *
rem ***************************************************

rem location of the upper folder:
set _devFolder="C:\Dev"

set _svn="C:\Program Files\SlikSvn\bin\svn.exe"
IF NOT EXIST %_svn% set _svn="D:\Program Files\SlikSvn\bin\svn.exe"
set _logDir=%_devFolder%\MapWindow4Dev\WeeklyBuilds\Logs\svn

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir%

REM Clean-up logfiles:
del /q %_logDir%\*.*

set _source=MapWindow4Dev
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%

set _source=MapWinGIS4Dev
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%

set _source=MapWindow4Plugins
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%

set _source=TestingScripts
set _log=%_logDir%\%_source%.log
IF EXIST %_devFolder%\%_source%\ (
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%
)

set _source=BASINS40
set _log=%_logDir%\%_source%.log
IF EXIST %_devFolder%\%_source%\ (
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%
)

set _source=D4EM
set _log=%_logDir%\%_source%.log
IF EXIST %_devFolder%\%_source%\ (
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%
)

set _source=SwatObject
set _log=%_logDir%\%_source%.log
IF EXIST %_devFolder%\%_source%\ (
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full %_devFolder%\%_source%\ >>%_log%
)


REM set mwSupportLibraries and mwFolder as an environment vars 
REM if you have the source code in different locations:
if "%mwSupportLibraries%"=="" set mwSupportLibraries=%_devFolder%\SupportLibraries
set _source=SupportLibraries
set _log=%_logDir%\%_source%.log
echo [%DATE% %Time%] Start SVN update for %_source% >%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\bin" >>%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\lib" >>%_log%
%_svn% update --accept=theirs-full "%mwSupportLibraries%\include" >>%_log%

REM open log files folder 
explorer %_logDir%
