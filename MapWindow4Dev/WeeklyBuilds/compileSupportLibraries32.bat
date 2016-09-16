rem ************************************************
rem * Script to compile all supporting libraries   *
rem * It calls the main compile script but sets    *
rem * the build option: 32-bit or 64-bit           *
rem * Created by: Paul Meems                       *
rem * 22 june 2011                                 *
rem ************************************************

set goodCall=ok

REM Supportlibraries
set buildSuppLib=/rebuild "Release|Win32"
set platform=Win32
set _logDir=C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\compile32

REM Call main compile script
echo Start compileSupportLibraries.bat
call compileSupportLibraries.bat delLog
