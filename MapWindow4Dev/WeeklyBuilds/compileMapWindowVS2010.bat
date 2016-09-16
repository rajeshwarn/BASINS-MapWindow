@echo off
rem ************************************************
rem * Script to compile all solutions of MapWindow *
rem * Created by: Paul Meems                       *
rem * Created 1 October 2009                       *
rem * Updated 5 October 2009                       *
rem * Updated 29 Aug. 2010: 64-Bit version is      *
rem *    .NET Framework v4 only and compiled with  *
rem *    VS2010 Pro                                *
rem * Updated 27 Oct. 2010: Adding compiling of    *
rem *   testingtool and BASINS40                   *
rem * Updated 19 Dec. 2010: Added compiling of     *
rem *   kigingplugin and moved BASINS40 to own file*
rem ************************************************

REM Check if correctly started:
if [%goodCall%]==[] goto wrong_call

rem location of the upper folder:
set _devFolder=C:\Dev

REM set environment variables:
set _devenv="%VS100COMNTOOLS%..\..\Common7\IDE\devenv"
set _logDir=%_devFolder%\MapWindow4Dev\WeeklyBuilds\Logs\compile64
set _log=%_logDir%\compileResults.log
set mwFolder=%_devFolder%\MapWindow4Dev
set mwSupportLibraries=%_devFolder%\SupportLibraries

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir% >>%_log%

REM Clean-up logfiles:
IF [%1]==[delLog] del /q %_logDir%\*.* >>%_log%

echo [%DATE% %Time%] Start compile sequence >%_log%
echo Used compile configuration is %buildX64% >>%_log%

REM Start Support libaries ************************************************
REM Don't call every time:
rem call compileSupportLibraries64.bat
echo Copy new supporting libraries >%_log%
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\x64\*.dll %mwFolder%\bin64\*.* >>%_log%
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\x64\PROJ_NAD\*.* %mwFolder%\bin64\PROJ_NAD\*.* >>%_log%
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\x64\gdal_data\*.* %mwFolder%\bin64\gdal_data\*.* >>%_log%
if not %errorlevel% == 0 echo Copying supporting libraries failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Copying supporting libraries was successful >>%_log%
REM End Support libaries ************************************************

REM Start MapWindow ActiveX ************************************************

REM ActiveX *
set _solutionPath=%_devFolder%\MapWinGIS4Dev
set _solutionName=MapWinGIS10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildOCXWin64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM If core binaries failed stop processing:
if not %errorlevel% == 0 (pause exit)

REM Start MapWindow Core ************************************************

Rem MapWinInterfaces *
set _solutionPath=%_devFolder%\MapWindow4Dev\MapWinInterfaces
set _solutionName=MapWinInterfaces10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem MapWinUtility * 
set _solutionPath=%_devFolder%\MapWindow4Plugins\MapWinUtility
set _solutionName=MapWinUtility10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem MapWinGeoProc *
set _solutionPath=%_devFolder%\MapWindow4Dev\MapWinGeoProc
set _solutionName=MapWinGeoProc10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Core plugins * 
set _solutionPath=%_devFolder%\MapWindow4Dev
set _solutionName=MWPluginsCS10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

set _solutionName=MWPluginsVB10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Script Editor
set _solutionPath=%_devFolder%\MapWindow4Dev\CSharpCodeCompletion
set _solutionName=CSharpCodeCompletion10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Core application *
set _solutionPath=%_devFolder%\MapWindow4Dev
set _solutionName=MapWindow10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
REM End MapWindow Core ************************************************

REM Additional plug-ins
Rem Labeler: This project is also in MWPluginsCS.sln. So no need to compile it again.

Rem PostgisPlugin *
rem Out-dated

Rem mwTaudemBasinsWrap
rem Doesn't work
	
Rem GraphPlugin *
set _solutionPath=%_devFolder%\MapWindow4Plugins\GraphPlugin
set _solutionName=GraphPlugin10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem MeemsTools *
set _solutionPath=%_devFolder%\MapWindow4Plugins\MeemsTools
set _solutionName=MeemsTools10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwBayesianNet
rem Can't compile, missing references

Rem mwBayesianNetSmile *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwBayesianNetSmile
set _solutionName=mwBayesianNetSmile10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwCSVtoShapefile *
rem depricated. Use spatial converter now

Rem mwDash
REM HydroObjects.WebServiceWrapper is not defined.

Rem GeoCoder *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwGoogleGeocoder
set _solutionName=GeoCoder10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwGPS *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwGPS
set _solutionName=mwGPS10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwGPSProximity *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwGPSProximity
set _solutionName=mwGPSProximity10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwGPX *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwGPX
set _solutionName=mwGPX10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwSampling *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwSampling
set _solutionName=mwSampling10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem GPS-Plugin
rem Missing WMXGPSLibrary.Net

rem HDAM
rem missing files of folder DataRetrieval

rem mwLabelmover *
rem depricated in favor of Symbology plug-in

rem mwLaunch *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwLaunch
set _solutionName=mwLaunch10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwMapServerGenerator *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwMapServerGenerator
set _solutionName=mwMapServerGenerator10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem measureProject *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwMeasureTool
set _solutionName=measureProject10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwOpenMetadataManager *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwOpenMetadataManager
set _solutionName=mwOpenMetadataManager10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwPathAnalyzer *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwPathAnalyzer
set _solutionName=mwPathAnalyzer10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem PhotoViewer *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwPhotoViewer
set _solutionName=PhotoViewer10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem ShapefileToGrid *
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwShapefileToGrid
set _solutionName=ShapefileToGrid10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwTINCreatorViewer

rem OnlineDataPlugin 
set _solutionPath=%_devFolder%\MapWindow4Plugins\OnlineDataPlugin
set _solutionName=OnlineDataPlugin10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem openhydro-alpha

rem OpenSWAT
rem missing references: ADOX, MS-ReportViewer

rem WatershedReport

rem mwSWAT  ----------
REM Missing Taudem reference
rem set _solutionPath=%_devFolder%\MapWindow4Plugins\MWSWAT
rem set _solutionName=mwSwat.sln
rem call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mw3DViewer: Need DirectX SDK (500MB)
set _solutionPath=%_devFolder%\MapWindow4Plugins\mw3DViewer
set _solutionName=mw3DViewer10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwLayout ---
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwLayout
set _solutionName=mwLayout10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Sam's BalloonIdentifier
set _solutionPath=%_devFolder%\MapWindow4Plugins\BalloonIdentifier
set _solutionName=mwBalloonIdentifier10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Gerd's Image Rectifier
set _solutionPath=%_devFolder%\MapWindow4Plugins\GRD_Tools\ImgRegPlugin
set _solutionName=ImgRegPlugin10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Spatial Converter
set _solutionPath=%_devFolder%\MapWindow4Plugins\SpatialConverter
set _solutionName=SpatialConverter10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwTiles
set _solutionPath=%_devFolder%\MapWindow4Plugins\mwTiles
set _solutionName=mwTiles10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Symbology plug-in
set _solutionPath=%_devFolder%\MapWindow4Dev\SymbologyPlugin
set _solutionName=mwSymbology10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem TestingTool:
set _solutionPath=%_devFolder%\TestingScripts
set _solutionName=TestingScripts10.sln
IF EXIST %_solutionPath%\%_solutionName% ( 
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

rem Kriging plug-in
set _solutionPath=C:\Dev\MapWindow4Plugins\KrigingPlugin
set _solutionName=KrigingPlugin10.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildX64%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

echo [%DATE% %Time%] Finished compile sequence >>%_log%

REM BASINS40 script
REM **** Must be last because variable may be changed it the script  ****
echo [%DATE% %Time%] Calling compileBasins40_64.bat >>%_log%
compileBasins40_64.bat

goto :eof

:wrong_call
echo This script cannot be started on its own!
echo Use compileMapWindow##-debug.bat or compileMapWindow##-release.bat instead
pause



