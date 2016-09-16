@echo off
rem ************************************************
rem * Script to compile solutions for BASINS40     *
rem * Created by: Paul Meems                       *
rem * Created 19 Dec. 2010                         *
rem ************************************************

REM set environment variables:
IF "%goodCall%"=="" (
SET _devFolder=C:\Dev
SET buildX64=/build "Debug|x64"
set _devenv="%VS100COMNTOOLS%..\..\Common7\IDE\devenv"
set _logDir=%_devFolder%\MapWindow4Dev\WeeklyBuilds\Logs\compile64
set _log=%_logDir%\compileResults.log
)

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir% >>%_log%

REM ZedGraph:
set _solutionPath=%_devFolder%\BASINS40\atcZedgraphExperimental\ZedGraph 5.1 (.net 2)
set _solutionName=ZedGraph10.sln
set _fullPath="%_solutionPath%\%_solutionName%"
IF EXIST %_fullPath% ( 
call %_devenv% "%_solutionPath%\%_solutionName%" %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

REM Utilities, EDDT, WebDownload, DataCache:
set _solutionPath=%_devFolder%\D4EM\EDDT
set _solutionName=NASA-WorldWind10.sln
IF EXIST %_solutionPath%\%_solutionName% ( 
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

REM D4EM-DataManager:
set _solutionPath=%_devFolder%\D4EM\DataManager
set _solutionName=BASINS-DataManager10.sln
IF EXIST %_solutionPath%\%_solutionName% ( 
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

rem BASINS40:
set _solutionPath=%_devFolder%\BASINS40
set _solutionName=BASINS40_10.sln
IF EXIST %_solutionPath%\%_solutionName% ( 
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

:eof
echo [%DATE% %Time%] Finished BASINS4 compile sequence >>%_log%