rem ************************************************
rem * Script to compile all supporting libraries   *
rem * It calls the main compile script but sets    *
rem * the build option: 32-bit or 64-bit           *
rem * Created by: Paul Meems                       *
rem * 22 june 2011                                 *
rem ************************************************

set goodCall=ok

REM Supportlibraries
set buildSuppLib=/rebuild "Release|x64"
set platform=x64
set platform2=Win64
set _logDir=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\compile64

REM Call main compile script
echo Start compileSupportLibraries.bat
call compileSupportLibraries.bat delLog
