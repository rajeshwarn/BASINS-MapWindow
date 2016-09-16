@echo off
rem ************************************************
rem * Script to create new binaries for MapWindow  *
rem * in a weekly manner                           *
rem * Created by: Paul Meems                       *
rem * 12 May 2009                                  *
rem ************************************************

rem update source code
call updateSVN.bat

rem delete old logfiles:
del /q C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\compile32\*.*
rem del /q C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\svn\*.*

rem compile all solutions
call compileMapWindow32-debug.bat

rem commit new binaries:
cd C:\Dev\MapWindow4Dev\WeeklyBuilds\
call commitBin32.bat

rem commit patch:
cd C:\Dev\MapWindow4Dev\WeeklyBuilds\
rem call patch.bat

pause
