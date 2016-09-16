rem @echo off
rem ************************************************
rem * Script to create new binaries for MapWindow  *
rem * in a weekly manner                           *
rem * Created by: Paul Meems                       *
rem * 12 May 2009                                  *
rem * Copied on 1 Nov 2010 for x64 binaries        *
rem ************************************************

rem update source code
call updateSVN.bat

rem delete old logfiles:
del /q C:\Dev\MapWindow4Dev\WeeklyBuilds\Logs\compile64\*.*

rem compile all solutions
call compileMapWindow64-debug.bat

rem commit new binaries:
cd C:\Dev\MapWindow4Dev\WeeklyBuilds\
call commitBin64.bat

rem commit new BASINS binaries:
cd C:\Dev\MapWindow4Dev\WeeklyBuilds\
call commitBasins64.bat


pause
