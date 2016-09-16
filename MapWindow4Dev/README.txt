Project File Overview:

  MapWindow.sln

    This is the Visual Studio 2008 project file including
    only the MapWindow application only, the "core mapwindow".


  MapWindow10.sln

    This is the Visual Studio 2010 project file including
    only the MapWindow application only, the "core mapwindow".

  MWPluginsCS.sln

    This is the Visual Studio 2008 project file including
    all of the associated MapWindow plugins that ship with
    the "base" MapWindow project and installation. This includes
    things like feature identifier, gistools, etc. The C# plugins
    are separated out into this project file.

  MWPluginsVB.sln

    This is the Visual Studio 2008 project file including
    all of the associated MapWindow plugins that ship with
    the "base" MapWindow project and installation. This includes
    things like feature identifier, gistools, etc. The VB.Net plugins
    are separated out into this project file.

Binaries:

  The Bin subdirectory contains a release-mode build. All binaries
  committed here should be built with Visual Studio 2008. With version
  4.7 and up, MapWindow is no longer supported from Visual Studio 2005.

  After version 4.7SR, MapWindow requires .NET3.5
 
You should be able to compile MapWindow.sln and the plug-ins using VS2008Express, 
but for MapWinGIS.sln you need VS2008 Pro because else you don't have the MFC stuff.

Thank you!

Any questions/comments? Use the MapWindow forum: http://forum.mapwindow.org

Mailing lists are available at:
http://lists.mapwindow.org