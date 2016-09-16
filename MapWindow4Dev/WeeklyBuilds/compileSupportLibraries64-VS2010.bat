@echo off
rem ************************************************
rem * Script to compile all supporting libraries   *
rem * for MapWindow                                *
rem * Created by: Paul Meems                       *
rem * 01 October 2009                              *       
rem * Updated 29 Aug. 2010: 64-Bit version is      *
rem *    .NET Framework v4 only and compiled with  *
rem *    VS2010 Pro
rem ************************************************

title Compile MW Supporting libraries - 64Bit

echo Set environment variables:
set _devenv="%VS100COMNTOOLS%..\..\Common7\IDE\devenv"
REM set mwSupportLibraries and mwFolder as an environment vars 
REM if you have the source code in different locations:
if "%mwSupportLibraries%"=="" set mwSupportLibraries=C:\Dev\SupportLibraries
if "%mwFolder%"=="" set mwFolder=C:\Dev\MapWindow4Dev

REM these values might have been set by a calling bacthfile
if "%_logDir%"=="" set _logDir=%mwFolder%\WeeklyBuilds\Logs\compile64
if "%_log%"=="" set _log=%_logDir%\compileResults.log
if "%buildSuppLib%"=="" set buildSuppLib=/rebuild "Release|x64"

echo [%DATE% %Time%] Start compiling supporting libraries >>%_log%
echo Used compile configuration is %buildSuppLib% >>%_log%

echo SharpDevelop 
REM This build also copies log4net.dll and Mono.Cecil.dll (both compiled for Any CPU)
set _solutionPath=%mwSupportLibraries%\SharpDevelop\src
set _solutionName=SharpDevelop10.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo cqlib
set _solutionPath=%mwSupportLibraries%\cqlib
set _solutionName=cqlib_vc10.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo GEOS
set _solutionPath=%mwSupportLibraries%\GDAL\Geos\build\msvc10
set _solutionName=geos.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo LIBECW
echo Can't compile in VS2010
rem set _solutionPath=%mwSupportLibraries%\GDAL\libecwj2-3.3\Source\NCSBuildQmake
rem set _solutionName=libecwj2_win32_net_shared-v10.sln
rem echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
rem call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo XERCES
set _solutionPath=%mwSupportLibraries%\GDAL\xerces-c\projects\Win32\VC10\xerces-all
set _solutionName=xerceslib.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo Proj4
set _solutionPath=%mwSupportLibraries%\GDAL\proj\build\msvc10
set _solutionName=proj.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo lti_dsdk_dll.dll
echo Not compatible with VS2010 (VC++2010)
set _solutionName=lti_dsdk_dll.dll
rem echo [%DATE% %Time%] Start copying%_solutionName% >>%_logDir%\%_solutionName%.log
rem copy /Y %mwSupportLibraries%\GDAL\Geo_DSDK-64bit\lib\Release_md\lti_dsdk_dll.dll %mwSupportLibraries%\bin\x64 >>%_logDir%\%_solutionName%.log
rem copy /Y %mwSupportLibraries%\GDAL\Geo_DSDK-64bit\lib\Release_md\lti_dsdk_dll.lib %mwSupportLibraries%\lib\x64 >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% copied successful >>%_log%

echo GDAL
set _solutionPath=%mwSupportLibraries%\GDAL\gdal17
set _solutionName=makegdal10.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo IndexSearching
set _solutionPath=%mwSupportLibraries%\IndexSearching
set _solutionName=IndexSearching10.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo Weifen Luo Dock Panel
set _solutionPath=%mwSupportLibraries%\WeifenLuoDockPanel\Src
set _solutionName=WinFormsUI.Docking10.sln
echo [%DATE% %Time%] Start compiling %_solutionName% >>%_logDir%\%_solutionName%.log
call %_devenv% %_solutionPath%\%_solutionName% %buildSuppLib% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

 