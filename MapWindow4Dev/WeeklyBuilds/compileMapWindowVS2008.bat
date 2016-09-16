rem @echo off
rem ************************************************
rem * Script to compile all solutions of MapWindow *
rem * Created by: Paul Meems                       *
rem * Created 12 May 2009                          *
rem * Updated 1 October 2009                       *
rem * Updated Augst 2011                           *
rem ************************************************

rem rebuild ocx as well?
set _buildOCX=false

REM Check if correctly started:
if [%goodCall%]==[] goto wrong_call

REM set environment variables:
set _devenv="%VS90COMNTOOLS%..\..\Common7\IDE\devenv"
if "%mwFolder%"=="" set mwFolder=C:\Dev\MapWindow4Dev
if "%mwPluginsFolder%"=="" set mwPluginsFolder=C:\Dev\MapWindow4Plugins
set _logDir=%mwFolder%\WeeklyBuilds\Logs\compile32
if %platform%=="x64" set _logDir=%mwFolder%\WeeklyBuilds\Logs\compile64
set _log=%_logDir%\compileResults.log
REM if you have the support libraries in a different location:
if "%mwSupportLibraries%"=="" set mwSupportLibraries=C:\Dev\SupportLibraries

echo [%DATE% %Time%] Start compile sequence >%_log%
echo Used compile configuration is %buildConfig% >>%_log%

REM create log folder if not exists
IF NOT EXIST %_logDir% md %_logDir% >>%_log%

REM Clean-up logfiles:
IF [%1]==[delLog] del /q %_logDir%\*.* >>%_log%

REM Start Support libaries ************************************************
REM Don't call every time:
rem call compileSupportLibraries32.bat
REM Copy new supporting libraries:
set _outputfolder=bin
if %platform%=="x64" set _outputfolder=bin64
Rem copy all dlls:
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\%platform%\*.dll %mwFolder%\%_outputfolder%\ >>%_log%
Rem Remove some again:
del /Q %mwFolder%\%_outputfolder%\gdal18.dll >>%_log%
del /Q %mwFolder%\%_outputfolder%\xerces-c_3.dll >>%_log%
del /Q %mwFolder%\%_outputfolder%\NPlot.dll >>%_log%
del /Q %mwFolder%\%_outputfolder%\geos_c.dll >>%_log%
Rem copy proj data:
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\%platform%\PROJ_NAD\*.* %mwFolder%\%_outputfolder%\PROJ_NAD\*.* >>%_log%
xcopy /S /V /C /Q /R /Y %mwSupportLibraries%\bin\%platform%\gdal_data\*.* %mwFolder%\%_outputfolder%\gdal_data\*.* >>%_log%
if not %errorlevel% == 0 echo --------------- Copying supporting libraries failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo [%DATE% %Time%] Copying supporting libraries was successful >>%_log%
REM End Support libaries ************************************************

REM Start MapWindow Core ************************************************
REM ActiveX
set _solutionPath=C:\Dev\MapWinGIS4Dev
set _solutionName=MapWinGIS.sln
if %_buildOCX%==true (
	call %_devenv% %_solutionPath%\%_solutionName% /rebuild "Release|Win32" >>%_logDir%\%_solutionName%.log
	if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
	if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)
if not %_buildOCX%==true (if %errorlevel% == 0 echo %_solutionName% copied successful >>%_log%)

REM If ActiveX failed stop processing:
if not %errorlevel% == 0 goto eof

REM Create interop dlls
set _solutionPath=C:\dev\MapWindow4Dev\interopCreator
set _solutionName=interopCreator.sln
if %_buildOCX%==true (
	call %_devenv% %_solutionPath%\%_solutionName% /rebuild "Release|x86" >>%_logDir%\%_solutionName%.log
	if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
	if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
)

Rem MapWinUtility
set _solutionPath=%mwPluginsFolder%\MapWinUtility
set _solutionName=MapWinUtility.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem MapWinInterfaces
set _solutionPath=%mwFolder%\MapWinInterfaces
set _solutionName=MapWinInterfaces.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem MapWinGeoProc
set _solutionPath=%mwFolder%\MapWinGeoProc
set _solutionName=MapWinGeoProc.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM mwControls
set _solutionPath=%mwFolder%\Controls
set _solutionName=mwControls.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%


REM Core plugins
set _solutionPath=%mwFolder%
set _solutionName=MWPluginsCS.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

set _solutionPath=%mwFolder%
set _solutionName=MWPluginsVB.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Script Editor
set _solutionPath=%mwFolder%\CSharpCodeCompletion
set _solutionName=CSharpCodeCompletion.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

REM Core application
set _solutionPath=%mwFolder%
set _solutionName=MapWindow.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
REM End MapWindow Core ************************************************


REM Additional plug-ins

Rem PostgisPlugin
set _solutionPath=%mwFolder%\PostgisPlugin
set _solutionName=DatabasePlugin.sln
rem is not shipped anymore:
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%
	
Rem GraphPlugin
set _solutionPath=%mwPluginsFolder%\GraphPlugin
set _solutionName=GraphPlugin.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem MeemsTools
set _solutionPath=%mwPluginsFolder%\MeemsTools
set _solutionName=MeemsTools.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwBayesianNet
rem Can't compile, missing references

Rem mwBayesianNetSmile
rem Throws error on loading
rem set _solutionPath=%mwPluginsFolder%\mwBayesianNetSmile
rem set _solutionName=mwBayesianNetSmile.sln
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwCSVtoShapefile
set _solutionPath=%mwPluginsFolder%\mwCSVtoShapefile
set _solutionName=mwCSVtoShapefile.sln
rem Replaced by Spatial Converter:
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwDash
REM HydroObjects.WebServiceWrapper is not defined.

Rem GeoCoder
set _solutionPath=%mwPluginsFolder%\mwGoogleGeocoder
set _solutionName=GeoCoder.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwGPS
set _solutionPath=%mwPluginsFolder%\mwGPS
set _solutionName=mwGPS.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

Rem mwGPSProximity
set _solutionPath=%mwPluginsFolder%\mwGPSProximity
set _solutionName=mwGPSProximity.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwGPX
set _solutionPath=%mwPluginsFolder%\mwGPX
set _solutionName=mwGPX.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwSampling 
set _solutionPath=%mwPluginsFolder%\mwSampling
set _solutionName=mwSampling.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem GPS-Plugin
rem Missing WMXGPSLibrary.Net
rem set _solutionPath=%mwPluginsFolder%\GPSPlugin
rem set _solutionName=GPS_Plugin.sln
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%


rem HDAM
rem missing files of folder DataRetrieval

rem mwLabelmover
rem Deprecated. Replaced by label mover of mwSymbology
rem set _solutionPath=%mwPluginsFolder%\mwLabelmover
rem set _solutionName=mwLabelmover.sln
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwLaunch
set _solutionPath=%mwPluginsFolder%\mwLaunch
set _solutionName=mwLaunch.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwMapServerGenerator
set _solutionPath=%mwPluginsFolder%\mwMapServerGenerator
set _solutionName=mwMapServerGenerator.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem measureProject
set _solutionPath=%mwPluginsFolder%\mwMeasureTool
set _solutionName=measureProject.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwOpenMetadataManager
set _solutionPath=%mwPluginsFolder%\mwOpenMetadataManager
set _solutionName=mwOpenMetadataManager.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwPathAnalyzer
set _solutionPath=%mwPluginsFolder%\mwPathAnalyzer
set _solutionName=mwPathAnalyzer.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem PhotoViewer
set _solutionPath=%mwPluginsFolder%\mwPhotoViewer
set _solutionName=PhotoViewer.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem ShapefileToGrid
set _solutionPath=%mwPluginsFolder%\mwShapefileToGrid
set _solutionName=ShapefileToGrid.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwTINCreatorViewer

rem OnlineDataPlugin
set _solutionPath=%mwPluginsFolder%\OnlineDataPlugin
set _solutionName=OnlineDataPlugin.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem openhydro-alpha

rem OpenSWAT
rem missing references: ADOX, MS-ReportViewer

rem AWD v3
set _solutionPath=%mwPluginsFolder%\AWD_V3
set _solutionName=AWD_V3.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwSWAT
set _solutionPath=%mwPluginsFolder%\MWSWAT
set _solutionName=mwSwat.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwSWAT2009
set _solutionPath=%mwPluginsFolder%\MWSWAT2009
set _solutionName=MWSWAT2009.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mw3DViewer: Need DirectX SDK (500MB)
rem set _solutionPath=%mwPluginsFolder%\mw3DViewer
rem set _solutionName=mw3DViewer.sln
rem call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
rem if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
rem if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwLayout
set _solutionPath=%mwPluginsFolder%\mwLayout
set _solutionName=mwLayout.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Sam's BalloonIdentifier
set _solutionPath=%mwPluginsFolder%\BalloonIdentifier
set _solutionName=MWBalloonIdentifier.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Gerd's Image Rectifier
rem set _solutionPath=%mwPluginsFolder%\GRD_Tools\ImgRegPlugin
rem set _solutionName=ImgRegPlugin.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Gerd's Image Rectifier - C# version
set _solutionPath=%mwPluginsFolder%\GRD_Tools\ImgRegPluginCSharp
set _solutionName=ImgRegPluginCSharp.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig% >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Spatial Converter
set _solutionPath=%mwPluginsFolder%\SpatialConverter
set _solutionName=SpatialConverter.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem mwTiles
set _solutionPath=%mwPluginsFolder%\mwTiles
set _solutionName=mwTiles.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Symbology plug-in
set _solutionPath=%mwFolder%\SymbologyPlugin
set _solutionName=mwSymbology.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem Kriging plug-in
set _solutionPath=%mwPluginsFolder%\KrigingPlugin
set _solutionName=KrigingPlugin.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%

rem GridAnalysisTool
set _solutionPath=%mwPluginsFolder%\GridAnalysisTool
set _solutionName=GridAnalysisTool.sln
call %_devenv% %_solutionPath%\%_solutionName% %buildConfig%  >>%_logDir%\%_solutionName%.log
if not %errorlevel% == 0 echo --------------- %_solutionName% failed!   Error: %errorlevel% >>%_log%
if %errorlevel% == 0 echo %_solutionName% compiled successful >>%_log%


echo [%DATE% %Time%] Finished compile sequence >>%_log%

goto eof

:wrong_call
echo This script cannot be started on its own!
echo Use compileMapWindow##-debug.bat or compileMapWindow##-release.bat instead
pause

:eof
