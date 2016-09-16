rem ************************************************
rem * Script to compile all solutions of MapWindow *
rem * It calls the main compile script but sets    *
rem * the build option: debug or release           *
rem * Created by: Paul Meems                       *
rem * 13 August 2009                               *
rem ************************************************

REM The solutions have different configurations names
REM so set them all. The main compile script will use them

set goodCall=ok

REM Supportlibraries
set buildSuppLib=/build "Release|x64"
set buildConfig=/build "Debug|x64"

REM Set log dir:
set _logDir=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\compile64

set platform="x64"

REM Call main compile script
call compileMapWindowVS2008.bat
