rem ************************************************
rem * Script to compile all supporting libraries   *
rem * for MapWindow                                *
rem * Created by: Paul Meems                       *
rem * 01 October 2009                              *
rem * Modified by Paul Meems on 23 June 2011       *
rem ************************************************
@echo off

title Compile MW Supporting libraries

REM Check if correctly started:
if [%goodCall%]==[] goto wrong_call

REM set environment variables:
set _devenv="%VS90COMNTOOLS%..\..\Common7\IDE\devenv"
REM set mwSupportLibraries and mwFolder as an environment vars 
REM if you have the source code in different locations:
if "%mwSupportLibraries%"=="" set mwSupportLibraries=C:\Dev\SupportLibraries
if "%mwFolder%"=="" set mwFolder=C:\Dev\MapWindow4Dev

REM these values might have been set by a calling batchfile
if "%_logDir%"=="" set _logDir=%mwFolder%\WeeklyBuilds\Logs\compile32
if "%_log%"=="" set _log=%_logDir%\compileResults.log

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir% >>%_log%

REM Clean-up logfiles:
IF [%1]==[delLog] del /q %_logDir%\*.* >>%_log%

echo [%DATE% %Time%] Start compiling supporting libraries >%_log%
echo Used compile configuration is %buildSuppLib% >>%_log%

REM SharpDevelop *
REM This build also copies log4net.dll and Mono.Cecil.dll
set _solutionPath=%mwSupportLibraries%\SharpDevelop\src
set _solutionName=SharpDevelop.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM cqlib *
set _solutionPath=%mwSupportLibraries%\cqlib
set _solutionName=cqlib.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM GEOS
set _solutionPath=%mwSupportLibraries%\GDAL\Geos
set _solutionName=makegeos90.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM ECW *
REM Copy from C:\dev\SupportLibraries\GDAL\ERDAS-ECW-JP2-SDK
set _solutionPath=%mwSupportLibraries%\GDAL\ERDAS-ECW-JP2-SDK
set _solutionName=ERDAS-ECW-JP2-SDK
echo [%DATE% %Time%] Start copying%_solutionName% >>%_logDir%\%_solutionName%.log     
xcopy /v /c /r /y %_solutionPath%\redistributable\vc90\%platform%\*.dll %mwSupportLibraries%\bin\%platform% >>%_logDir%\%_solutionName%.log
xcopy /v /c /r /y %_solutionPath%\lib\vc90\%platform%\*.lib %mwSupportLibraries%\lib\%platform% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM XERCES *
set _solutionPath=%mwSupportLibraries%\GDAL\xerces-c\projects\Win32\VC9\xerces-all
set _solutionName=xerceslib.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Proj4 *
set _solutionPath=%mwSupportLibraries%\GDAL\proj\build\msvc90
set _solutionName=proj.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM LizardTech-SDK *
REM No source code available
set _solutionName=LizardTech-SDK
echo [%DATE% %Time%] Start copying%_solutionName% >>%_logDir%\%_solutionName%.log
copy /Y %mwSupportLibraries%\GDAL\LizardTech-SDK\%platform2%\Lidar_DSDK\lib\lti_lidar_dsdk.dll %mwSupportLibraries%\bin\%platform% >>%_logDir%\%_solutionName%.log
copy /Y %mwSupportLibraries%\GDAL\LizardTech-SDK\%platform2%\Lidar_DSDK\lib\lti_lidar_dsdk.lib %mwSupportLibraries%\lib\%platform% >>%_logDir%\%_solutionName%.log
copy /Y %mwSupportLibraries%\GDAL\LizardTech-SDK\%platform2%\Raster_DSDK\lib\lti_dsdk.dll %mwSupportLibraries%\bin\%platform% >>%_logDir%\%_solutionName%.log
copy /Y %mwSupportLibraries%\GDAL\LizardTech-SDK\%platform2%\Raster_DSDK\lib\lti_dsdk.lib %mwSupportLibraries%\lib\%platform% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM GDAL *
set _solutionPath=%mwSupportLibraries%\GDAL\gdal18
set _solutionName=makegdal90.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM IndexSearching *
set _solutionPath=%mwSupportLibraries%\IndexSearching
set _solutionName=IndexSearching.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Weifen Luo Dock Panel *
set _solutionPath=%mwSupportLibraries%\WeifenLuoDockPanel\Src
set _solutionName=WinFormsUI.Docking.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Paul Meems 25 Feb. 2010
REM Moved this project into the MapWindow solution, so no need to compile it separately
REM ToolStripEx (and MenuStripEx) *
REM set _solutionName=ToolStripEx
REM echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
REM pushd %mwSupportLibraries%\ToolStripEx
REM set BIN=..\bin\Win32
REM "%VS90COMNTOOLS%..\..\VC\bin\nmake.exe" /E
REM popd

echo [%DATE% %Time%] Finished compile sequence >>%_log%

goto eof

:wrong_call
echo This script cannot be started on its own!
echo Use compileSupportLibraries32.bat or compileSupportLibraries64.bat instead
pause

:eof
