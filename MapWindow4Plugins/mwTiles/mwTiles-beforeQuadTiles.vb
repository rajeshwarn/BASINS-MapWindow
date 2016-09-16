
''' <summary>
''' Creates a new Tiles menu for viewing tiles from web-based maps in the background of the MapWindow project.
''' Core code is copied from VataviaMap at http://code.google.com/p/vataviamap/
''' Initial implementation loads tiles from OpenStreetMap.org
''' </summary>
''' <remarks></remarks>
Public Class mwTiles
    Implements MapWindow.Interfaces.IPlugin, IQueueListener

    Public g_MapWin As MapWindow.Interfaces.IMapWin
    Public g_handle As Integer
    Public g_DrawingHandle As Integer = -1
    Private pEnabled As Boolean = False
    Private pIsValidProject As Boolean = False
    Private pTilesNeeded As Integer = 0
    Private pCurProgress As Integer = 0

    Public Const TileProjectionProj4 As String = "+proj=merc +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +units=m +no_defs" 'EPSG:900913
    Public Const GeographicProjectionProj4 As String = "+proj=latlong +datum=NAD83"
    Public Const EPSG3785Proj4 As String = "+proj=merc +lon_0=0 +k=1 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs" 'EPSG:3785
    Public Const TileProjectionNAD83Proj4 As String = "+proj=merc +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +datum=NAD83 +units=m"
    Public Const TileProjPseudoMercator As String = "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs"

    ' Paul Meems Added:
    'Public ZoomList As System.Collections.Generic.IList(Of Zoomlevels) = New System.Collections.Generic.List(Of Zoomlevels)()

    Private Const pTileMenuName As String = "mnuTiles"
    Private Const pTileMenuNone As String = pTileMenuName & "_None"
    Private Const pTileMenuCache As String = pTileMenuName & "_Cache"

    Private pTileLayersMutex As New Threading.Mutex
    Private Downloader As New clsDownloader

    Private Servers As New Generic.Dictionary(Of String, clsServer)

    Private pZoom As Integer = 0
    Private pTileCacheFolder As String        'Global tile cache folder with Etag filename extensions and no associated files (.pgw, .prj)
    Private pProjectTileCacheFolder As String 'Local copy of tiles named with image extension (.png) and associated files (.pgw, .prj)

    Private pFinishedQueue As Boolean = True

#Region "Plug-in Information"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Tiled Map"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "Mark Gray"
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType.Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType.Assembly.Location).FileVersion
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "Display Tiled Map In Background"
        End Get
    End Property

#End Region
#Region "Initialize and Terminate"

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        g_MapWin = MapWin  '  This sets global for use elsewhere in program
        g_handle = ParentHandle

        g_ShowStatusProgress = GetAppSetting("ShowStatusProgress", Not g_MapWin.ApplicationInfo.ApplicationName.StartsWith("BASINS"))

        Dim lTileCacheBaseName As String = "tiles"
        'Try to share VataviaMap or BASINS cache folder, if not found then create tiles folder in user's temp space
        pTileCacheFolder = GetAppSetting("TileCacheFolder", IO.Path.GetTempPath() & lTileCacheBaseName)
        If Not IO.Directory.Exists(pTileCacheFolder) Then
            Dim lBasinsCacheDir As String = IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)) & g_PathChar & "cache"
            If IO.Directory.Exists(lBasinsCacheDir) Then
                pTileCacheFolder = lBasinsCacheDir & g_PathChar & lTileCacheBaseName
            End If
        End If
        If Not pTileCacheFolder.EndsWith(g_PathChar) Then pTileCacheFolder &= g_PathChar

        SetDefaultTileServers()

        g_TileCacheFolder = pTileCacheFolder & SafeFilename(g_TileServer.Name.Replace(" ", "")) & g_PathChar

        With g_MapWin.Menus
            '.AddMenu(pZoomMenuName, "", Nothing, "Zoom", "mnuFile")
            'For lZoom As Integer = g_TileServer.ZoomMin To g_TileServer.ZoomMax
            '    .AddMenu(pZoomMenuName & lZoom, pZoomMenuName, Nothing, CStr(lZoom))
            'Next
            .AddMenu(pTileMenuName, "", Nothing, "Tiles", "mnuFile")
            .AddMenu(pTileMenuNone, pTileMenuName, Nothing, "No Tiles")
            For Each lServer As clsServer In Servers.Values
                If Not String.IsNullOrEmpty(lServer.TilePattern) Then
                    .AddMenu(pTileMenuName & "_" & lServer.Name.Replace(" ", "_"), pTileMenuName, Nothing, lServer.Name)
                End If
            Next
            .AddMenu(pTileMenuCache, pTileMenuName, Nothing, "Cache Folder...")
        End With
        Downloader.TileRAMcacheLimit = 0 'Do not cache bitmaps in downloader, MW will keep the layers
        Downloader.Listeners.Add(Me)

        'Paul Meems Added:
        'FillZoomLevelList()
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        SaveAppSetting("ShowStatusProgress", g_ShowStatusProgress)

        g_MapWin.Menus.Remove(pTileMenuName) ' remove all menu items in this menu

        RemoveAllTiles(True)
    End Sub

#End Region

    Private Property Enabled() As Boolean
        Get
            Return pEnabled
        End Get
        Set(ByVal value As Boolean)
            pEnabled = value
            'Downloader.Enabled = pEnabled
            Threading.Thread.Sleep(50) 'Give other thread a chance to finish
        End Set
    End Property

    ''' <summary>
    ''' Extracts an embedded file out of a given assembly as a string
    ''' </summary>
    ''' <param name="aAssembly">Assembly file is embedded in.</param>
    ''' <param name="fileName">Name of the file to extract.</param>
    ''' <returns>A string containing the file data.</returns>
    Public Function GetEmbeddedFileAsString(ByVal fileName As String, Optional ByVal aAssembly As Reflection.Assembly = Nothing) As String
        If aAssembly Is Nothing Then aAssembly = Reflection.Assembly.GetCallingAssembly
        Dim lReader As New IO.StreamReader(aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName))
        Return lReader.ReadToEnd()
    End Function

    Public Sub SetDefaultTileServers()

        'August 3, 2011 - Paul Meems: Always use embedded file:
        'Dim lTileServersFilename As String = pTileCacheFolder & "servers1.html"
        'Dim lServersHTML As String
        'If IO.File.Exists(lTileServersFilename) Then
        '    lServersHTML = ReadTextFile(lTileServersFilename)
        'Else
        '    'Downloader.DownloadFile("http://vatavia.net/mark/VataviaMap/servers.html", lTileServersFilename, lTileServersFilename, False) Then
        '    lServersHTML = GetEmbeddedFileAsString("servers.html")
        '    Try
        '        IO.File.WriteAllText(lTileServersFilename, lServersHTML)
        '    Catch
        '    End Try
        'End If

        Dim lServersHTML As String = GetEmbeddedFileAsString("servers.html")
        Servers = clsServer.ReadServers(lServersHTML)
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        Select Case ItemName
            Case pTileMenuCache
                If IO.Directory.Exists(pTileCacheFolder) Then
                    OpenFileOrURL(pTileCacheFolder, False)
                Else
                    MsgBox("Cache folder not yet created", MsgBoxStyle.OkOnly, Me.Name)
                End If
            Case pTileMenuNone
                RemoveAllTiles(False)
                Enabled = False
            Case Else
                If ItemName.StartsWith(pTileMenuName & "_") Then
                    Dim lMenuName As String = ItemName.Substring(pTileMenuName.Length + 1).Replace("_", " ")
                    Try
                        If Not pIsValidProject Then
                            'Check whether the project has become valid
                            pIsValidProject = IsValidProject(True)
                        End If
                        If pIsValidProject Then
                            pProjectTileCacheFolder = IO.Path.GetDirectoryName(g_MapWin.Project.FileName) & g_PathChar & "tiles" & g_PathChar

                            TileServerName = lMenuName
                            Enabled = True
                            EnsureCorrectTilesVisible(True)
                        End If
                    Catch e As Exception
                        MsgBox("Exception handling " & ItemName & vbCrLf _
                               & e.Message & vbCrLf _
                               & e.StackTrace, MsgBoxStyle.OkOnly, Me.Name)
                    End Try
                End If
        End Select
    End Sub

    Private Function IsValidProject(ByVal aDisplayProblemMessage As Boolean) As Boolean
        If g_MapWin IsNot Nothing _
           AndAlso g_MapWin.Project IsNot Nothing _
           AndAlso g_MapWin.View IsNot Nothing _
           AndAlso g_MapWin.View.Extents IsNot Nothing _
           AndAlso g_MapWin.View.Extents.xMax > g_MapWin.View.Extents.xMin Then

            Dim sProj As String = g_MapWin.Project.ProjectProjection.Trim()
            If sProj <> TileProjectionProj4 AndAlso sProj <> TileProjectionNAD83Proj4 AndAlso sProj <> TileProjPseudoMercator Then
                'AndAlso g_MapWin.Project.ProjectProjection <> EPSG3785Proj4 Then
                If aDisplayProblemMessage Then
                    MapWinUtility.Logger.Msg("Can only use tiles if project is in tile projection" & vbLf _
                           & "Name: WGS 84 / Pseudo Mercator (EPSG:3857)" & vbLf _
                           & "Unloading " & Me.Name & " Plugin.", MsgBoxStyle.OkOnly, Me.Name)
                End If
                Dim lKey As String = g_MapWin.Plugins.GetPluginKey(Me.Name)
                If Not String.IsNullOrEmpty(lKey) Then g_MapWin.Plugins.StopPlugin(lKey)
                Return False
            Else
                If g_MapWin Is Nothing OrElse g_MapWin.Project Is Nothing OrElse Not IO.File.Exists(g_MapWin.Project.FileName) Then
                    MsgBox("Project must be saved before adding tiles", MsgBoxStyle.OkOnly, Me.Name)
                    Return False
                End If
            End If
            Return True
        End If
        Return False
    End Function

    Private Sub SetCacheFolderFromTileServer()
        g_TileCacheFolder = pTileCacheFolder & SafeFilename(g_TileServer.Name.Replace(" ", "")) & g_PathChar
    End Sub

    Public Property TileServerName() As String
        Get
            Return g_TileServer.Name
        End Get
        Set(ByVal value As String)
            If value <> g_TileServer.Name Then
                If Servers.ContainsKey(value) Then
                    Downloader.ClearQueue(QueueItemType.TileItem, -1)
                    RemoveAllTiles(False)
                    g_TileServer = Servers(value)
                    'Me.Text = g_AppName & " " & g_TileServer.Name
                    SetCacheFolderFromTileServer()
                End If
            End If
        End Set
    End Property

    Private Sub EnsureCorrectTilesVisible(ByVal aCalculateZoom As Boolean)
        'Debug.WriteLine("In EnsureCorrectTilesVisible")
        If Not Enabled Then Exit Sub

        Try

            Dim lNorth, lSouth, lWest, lEast As Double
            GetGeographicBounds(lNorth, lSouth, lWest, lEast)

            Dim lOffsetFromTileEdge As Point

            If aCalculateZoom Then
                ' initialize zoom level:
                pZoom = g_TileServer.ZoomMin
            End If

            Dim lTopLeftTileInView As Point = CalcTileXY(lNorth, lWest, pZoom, lOffsetFromTileEdge)
            Dim lBotRightTileInView As Point = CalcTileXY(lSouth, lEast, pZoom, lOffsetFromTileEdge)

            If aCalculateZoom Then

                With g_MapWin.View
                    Dim lXMin As Double, lXMax As Double
                    Dim lYMin As Double, lYMax As Double
                    .ProjToPixel(.Extents.xMax, .Extents.yMax, lXMax, lYMax)
                    .ProjToPixel(.Extents.xMin, .Extents.yMin, lXMin, lYMin)
                    pTilesNeeded = 0.75 * (Math.Abs(lXMax - lXMin) / g_TileServer.TileSize) * (Math.Abs(lYMax - lYMin) / g_TileServer.TileSize)
                End With

                While pZoom < g_TileServer.ZoomMax AndAlso _
                      (1 + Math.Abs(lBotRightTileInView.X - lTopLeftTileInView.X)) * (1 + Math.Abs(lBotRightTileInView.Y - lTopLeftTileInView.Y)) < pTilesNeeded
                    pZoom += 1
                    lTopLeftTileInView = CalcTileXY(lNorth, lWest, pZoom, lOffsetFromTileEdge)
                    lBotRightTileInView = CalcTileXY(lSouth, lEast, pZoom, lOffsetFromTileEdge)
                End While


                ' Get zoomlevel from list:
                'Dim currentScale As Double = g_MapWin.View.Scale
                'Dim newZoom As Double = GetZoomLevel(currentScale)
                'Debug.WriteLine("currentScale: " & currentScale)
                'Dim zoomLevels As Zoomlevels = GetZoomLevel(currentScale)
                'Debug.WriteLine("Zoomlevel-new: " & zoomLevels.Zoomlevel)
                'Debug.WriteLine("Best scale: " & zoomLevels.Scale)
                'pZoom = zoomLevels.Zoomlevel
                lTopLeftTileInView = CalcTileXY(lNorth, lWest, pZoom, lOffsetFromTileEdge)
                lBotRightTileInView = CalcTileXY(lSouth, lEast, pZoom, lOffsetFromTileEdge)
            End If

            Dim lTilePoint As Point
            Dim lOffsetFromWindowCorner As Point

            Dim lTilePoints As New SortedList(Of Single, Point)
            Dim x, y As Integer
            Dim lMidX As Integer = (lTopLeftTileInView.X + lBotRightTileInView.X) / 2
            Dim lMidY As Integer = (lTopLeftTileInView.Y + lBotRightTileInView.Y) / 2
            Dim lDistance As Single

            'Sort visible tiles by distance from middle so we load ones near middle first
            For x = lTopLeftTileInView.X To lBotRightTileInView.X
                For y = lTopLeftTileInView.Y To lBotRightTileInView.Y
                    Try
                        lDistance = (x - lMidX) ^ 2 + (y - lMidY) ^ 2
                        While lTilePoints.ContainsKey(lDistance)
                            lDistance += 0.1 'SortedList requires unique keys
                        End While
                        lTilePoints.Add(lDistance, New Point(x, y))
                    Catch
                        lDistance = 11
                        While lTilePoints.ContainsKey(lDistance)
                            lDistance += 0.1
                        End While
                        lTilePoints.Add(lDistance, New Point(x, y))
                    End Try
                Next
            Next

            For Each lTilePoint In lTilePoints.Values
                'If Not Enabled Then Exit Sub
                Dim lDrewTile As Boolean = EnsureTileVisible(lTilePoint, pZoom, lOffsetFromWindowCorner, 0)
                'Debug.Print("EnsureTileVisible returned " & lDrewTile.ToString() & " for zoom " & pZoom.ToString())
            Next

            While Enabled AndAlso Downloader.RunQueue
                Threading.Thread.Sleep(10)
                Windows.Forms.Application.DoEvents()
            End While

            If Not Enabled Then Exit Sub
            pTileLayersMutex.WaitOne()

            Progress("Finished fetching tiles", -1, -1)
            pCurProgress = 0

            'Find all the tiles we just ensured are visible
            Dim lTileFilenamesToKeep As New Generic.List(Of String)
            For Each lTilePoint In lTilePoints.Values
                Dim lTileFileName As String = TileFilename(lTilePoint, pZoom, False)
                lTileFilenamesToKeep.Add(lTileFileName.ToLower)
            Next

            'Find tiles on map that we are not trying to keep visible
            Dim lHandlesToRemove As New Generic.List(Of Integer)
            For Each lLayer As MapWindow.Interfaces.Layer In g_MapWin.Layers
                If lLayer IsNot Nothing Then
                    Dim lTileFileName As String = lLayer.FileName.ToLower
                    If lTileFileName.StartsWith(g_TileCacheFolder) AndAlso Not lTileFilenamesToKeep.Contains(lTileFileName) Then
                        lHandlesToRemove.Add(lLayer.Handle)
                    End If
                End If
            Next
            'Remove tiles not on map (outside the loop above so we can use For Each)
            For Each lHandle As Integer In lHandlesToRemove
                g_MapWin.Layers.Remove(lHandle)
            Next

            pTileLayersMutex.ReleaseMutex()
            ' Paul Meems: Do we really need to redraw? If a lot of other data is loaded it is taking a while
            ' Refresh seems to do the same but faster:
            'g_MapWin.View.Redraw()
            g_MapWin.Refresh()
            'Debug.WriteLine("After Refresh in EnsureCorrectTilesVisible")
        Catch e As Exception
            MapWinUtility.Logger.Dbg("EnsureCorrectTilesVisible: " & e.ToString)
        End Try
    End Sub

    Private Function IsTileAlreadyLoaded(ByVal aTileFileName As String) As Boolean
        Try
            If g_TileCacheFolder IsNot Nothing AndAlso g_TileCacheFolder.Length > 0 Then
                aTileFileName = SafeSubstring(aTileFileName, g_TileCacheFolder.Length - 1).ToLower
            Else
                aTileFileName = aTileFileName.ToLower
            End If
            'Dim lTileGroupHandle As Integer = FindTileGroupHandle()

            For i As Integer = 0 To g_MapWin.Layers.NumLayers
                Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(i)
                Dim lLayer As MapWindow.Interfaces.Layer = g_MapWin.Layers(lLayerHandle)
                If lLayer IsNot Nothing AndAlso lLayer.FileName.ToLower.Contains(aTileFileName) Then
                    ' Paul Meems: Move the layer to top:
                    'MoveLayerToTop(lTileGroupHandle, lLayer)
                    Return True
                End If
            Next
            Return False

        Catch ex As Exception
            Debug.WriteLine("Error in IsTileAlreadyLoaded" & ex.ToString())
        End Try
    End Function

    Private Function EnsureTileVisible(ByVal aTilePoint As Point, ByVal aZoom As Integer, ByVal aOffset As Point, ByVal aPriority As Integer) As Boolean
        'Debug.WriteLine("In EnsureTileVisible")
        Dim lTileFileName As String = TileFilename(aTilePoint, aZoom, False).ToLower

        ' Paul Meems: Added extra check:
        If lTileFileName = String.Empty Then
            ' Most likely zoomed too fast
            Return False
        End If

        ' Update progress value
        pCurProgress += 1

        If IsTileAlreadyLoaded(lTileFileName) Then
            Progress("Showing tile", pCurProgress, pTilesNeeded)
            Return True
        End If

        Dim lActualTileFileName = Downloader.GetTileFilename(lTileFileName, aTilePoint, aZoom, aPriority, False)
        If IO.File.Exists(lActualTileFileName) Then
            Progress("Loading tile", pCurProgress, pTilesNeeded)
            Return AddTileToMap(aTilePoint, aZoom, lActualTileFileName)
        Else
            pFinishedQueue = False
            Progress("Downloading tile", pCurProgress, pTilesNeeded)
            Return False
        End If
    End Function

    Private Sub RemoveLayer(ByVal aLayer As MapWindow.Interfaces.Layer)
        For i As Integer = 0 To g_MapWin.Layers.NumLayers
            Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(i)
            If lLayerHandle >= 0 Then
                Try
                    If aLayer.FileName.Equals(g_MapWin.Layers(lLayerHandle).FileName) Then
                        g_MapWin.Layers.Remove(lLayerHandle)
                        Exit For
                    End If
                Catch lEx As Exception
                    Debug.Print("RemoveLayer " & lEx.Message & " at " & i & " " & lLayerHandle)
                End Try
            End If
        Next
    End Sub

    ''' <summary>
    ''' Remove all tiles from the map and from the project tile cache folder
    ''' </summary>
    ''' <remarks>Tiles are not removed from the main tile cache</remarks>
    Private Sub RemoveAllTiles(ByVal aRemoveTileGroup As Boolean)
        Enabled = False
        pTileLayersMutex.WaitOne()
        Downloader.ClearQueue()
        pFinishedQueue = True
        If aRemoveTileGroup Then
            Dim lTileGroupHandle As Integer = FindTileGroupHandle()
            If lTileGroupHandle >= 0 Then
                'Remove the group of tiles from the map, this removes all layers in the group
                g_MapWin.Layers.Groups.Remove(lTileGroupHandle)
            End If
        End If

        'Remove any tiles that are in the project tile cache but were not removed with the group above
        If IO.Directory.Exists(pProjectTileCacheFolder) Then
            For i As Integer = g_MapWin.Layers.NumLayers To 0 Step -1
                Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(i)
                If lLayerHandle >= 0 Then
                    Try
                        Dim lLayer As MapWindow.Interfaces.Layer = g_MapWin.Layers(lLayerHandle)
                        If lLayer.Name.StartsWith("mwTile-") OrElse lLayer.FileName.StartsWith(pProjectTileCacheFolder) Then
                            g_MapWin.Layers.Remove(lLayerHandle)
                        End If
                    Catch lEx As Exception
                        Debug.Print("RemoveLayer " & lEx.Message & " at " & i & " " & lLayerHandle)
                    End Try
                End If
            Next
            Try
                IO.Directory.Delete(pProjectTileCacheFolder, True)
            Catch
            End Try
        End If

        pTileLayersMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Sets parameters to the edges of the current MapWindow view in geographic coordinates
    ''' </summary>
    ''' <param name="aNorth">Northernmost latitude visible in current view</param>
    ''' <param name="aSouth">Southernmost latitude  visible in current view</param>
    ''' <param name="aWest">Westernmost longitude visible in current view</param>
    ''' <param name="aEast">Easternmost longitude visible in current view</param>
    ''' <remarks></remarks>
    Private Sub GetGeographicBounds(ByRef aNorth As Double, _
                                    ByRef aSouth As Double, _
                                    ByRef aWest As Double, _
                                    ByRef aEast As Double)

        Dim lSourceProj As String = g_MapWin.Project.ProjectProjection
        Dim lZmin, lZmax As Double

        If lSourceProj.Length > 0 AndAlso Not lSourceProj.Equals(GeographicProjectionProj4) Then
            Dim xNW, yNW As Double
            Dim xNE, yNE As Double
            Dim xSW, ySW As Double
            Dim xSE, ySE As Double

            With g_MapWin.View.Extents
                .GetBounds(xSW, ySW, lZmin, xNE, yNE, lZmax)
            End With
            xNW = xSW : xSE = xNE
            yNW = yNE : ySE = ySW

            MapWinGeoProc.SpatialReference.ProjectPoint(xNW, yNW, lSourceProj, GeographicProjectionProj4)
            MapWinGeoProc.SpatialReference.ProjectPoint(xNE, yNE, lSourceProj, GeographicProjectionProj4)
            MapWinGeoProc.SpatialReference.ProjectPoint(xSW, ySW, lSourceProj, GeographicProjectionProj4)
            MapWinGeoProc.SpatialReference.ProjectPoint(xSE, ySE, lSourceProj, GeographicProjectionProj4)

            If yNW > yNE Then aNorth = yNW Else aNorth = yNE
            If ySW < ySE Then aSouth = ySW Else aSouth = ySE

            If xNE > xSE Then aEast = xNE Else aEast = xSE
            If xSW < xNW Then aWest = xSW Else aWest = xNW
        Else
            With g_MapWin.View.Extents
                .GetBounds(aWest, aSouth, lZmin, aEast, aNorth, lZmax)
            End With
        End If
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
        'If pIsValidProject AndAlso Me.Enabled AndAlso IO.File.Exists(g_MapWin.Project.FileName) Then
        '    'Debug.WriteLine("In MapExtentsChanged")
        '    Windows.Forms.Application.DoEvents()
        '    pProjectTileCacheFolder = IO.Path.GetDirectoryName(g_MapWin.Project.FileName) & g_PathChar & "tiles" & g_PathChar
        '    EnsureCorrectTilesVisible(True)
        '    'Wait a moment:
        '    Threading.Thread.Sleep(100)
        'End If
    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
        If msg.ToUpper.Contains("PROJECT LOADED") Then
            pIsValidProject = IsValidProject(False)
            Enabled = pIsValidProject
            ' Get initial set of tiles:
            MapExtentsChanged()
        End If
    End Sub

    ''' <summary>
    ''' This is called when a project starts loading
    ''' </summary>
    ''' <param name="ProjectFile"></param>
    ''' <param name="SettingsString"></param>
    ''' <remarks></remarks>
    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        pIsValidProject = False
    End Sub


#Region "Unused Plug-in Interface Elements"

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
    End Sub

    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
    End Sub

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return ""
        End Get
    End Property

    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub

#End Region

    Public Sub DownloadedItem(ByVal aItem As clsQueueItem) Implements IQueueListener.DownloadedItem
        'mwTiles does not yet download any non-tile items
    End Sub

    Public Sub FinishedQueue(ByVal aQueueIndex As Integer) Implements IQueueListener.FinishedQueue
        pFinishedQueue = True
        Progress("Finished downloading tiles", -1, -1)
        pCurProgress = 0
    End Sub

    ''' <summary>
    ''' Called in downloader thread each time a tile is downloaded
    ''' </summary>
    ''' <remarks>can't add directly to map in this thread</remarks>
    Public Sub DownloadedTile(ByVal aTilePoint As System.Drawing.Point, ByVal aZoom As Integer, ByVal aFilename As String, ByVal aTileServerURL As String) Implements IQueueListener.DownloadedTile
        If Enabled AndAlso aTileServerURL = g_TileServer.TilePattern AndAlso IO.File.Exists(aFilename) Then
            AddTileToMap(aFilename)
            'pTileLayersMutex.WaitOne()
            'If String.IsNullOrEmpty(aFilename) Then
            '    Debug.WriteLine("Null or empty file name")
            'Else
            '    pTileLayersDownloaded.Add(aFilename)
            'End If
            'pTileLayersMutex.ReleaseMutex()
        End If
    End Sub

    'Private Sub AddDownloadedTilesToMap()
    '    If Enabled Then
    '        pTileLayersMutex.WaitOne()
    '        For Each lFileName As String In pTileLayersDownloaded
    '            AddTileToMap(lFileName)
    '        Next
    '        pTileLayersDownloaded.Clear()
    '        pTileLayersMutex.ReleaseMutex()
    '    End If
    'End Sub

    Private Sub AddTileToMap(ByVal aFilename As String)
        Dim lTemp As String = aFilename.Replace(pTileCacheFolder, "")
        Dim lParms() As String = lTemp.Split(g_PathChar)
        If lParms.Length > 3 Then
            Dim aZoom As Integer = lParms(1)
            Dim lParmsX() As String = lParms(3).Split(".")
            Dim aTilePoint As New System.Drawing.Point(lParms(2), lParmsX(0))
            AddTileToMap(aTilePoint, aZoom, aFilename)
        End If
    End Sub

    Private Function AddTileToMap(ByVal aTilePoint As System.Drawing.Point, ByVal aZoom As Integer, ByVal aFilename As String) As Boolean
        Try
            If Enabled Then
                'Cached tiles may not have .png extension or support files .pgw and .prj, so we copy cached files to project folder and give them support files while they are on the map
                Dim lLocalFilename As String = TileFilename(aTilePoint, aZoom, False).Replace(g_TileCacheFolder, pProjectTileCacheFolder)
                Dim lWorldFileExtension As String = "pgw"
                Dim lLowercaseFilename As String = IO.Path.GetFileName(aFilename).ToLower
                'Using Contains instead of GetExtension because .ETag extension may come after
                If lLowercaseFilename.Contains(".jpg") OrElse lLowercaseFilename.Contains(".jpeg") Then
                    lWorldFileExtension = "jgw"
                    lLocalFilename = IO.Path.ChangeExtension(lLocalFilename, "jpg")
                ElseIf lLowercaseFilename.Contains(".tif") Then
                    lWorldFileExtension = "tfw"
                    lLocalFilename = IO.Path.ChangeExtension(lLocalFilename, "tif")
                Else 'Default tile format is png
                    lWorldFileExtension = "pgw"
                    lLocalFilename = IO.Path.ChangeExtension(lLocalFilename, "png")
                End If
                If Not IO.File.Exists(lLocalFilename) Then
                    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lLocalFilename))
                    IO.File.Copy(aFilename, lLocalFilename)
                End If

                Dim lGeoReferenceFilename As String = IO.Path.ChangeExtension(lLocalFilename, lWorldFileExtension)
                If Not IO.File.Exists(lGeoReferenceFilename) Then
                    Dim lNorth As Double, lWest As Double, lSouth As Double, lEast As Double
                    CalcMetersFromTileXY(aTilePoint, aZoom, lNorth, lWest, lSouth, lEast)
                    CreateGeoReferenceFile(lNorth, lWest, aZoom, lGeoReferenceFilename)
                End If

                Dim lProjectionFilename As String = IO.Path.ChangeExtension(lLocalFilename, "prj")
                If Not IO.File.Exists(lProjectionFilename) Then
                    IO.File.WriteAllText(lProjectionFilename, TileProjectionProj4)
                End If

                If Enabled Then
                    Dim lImage As New MapWinGIS.Image
                    If lImage.Open(lLocalFilename, MapWinGIS.ImageType.USE_FILE_EXTENSION, True) Then
                        Try
                            pTileLayersMutex.WaitOne()
                            g_MapWin.View.LockLegend()
                            Dim lCurrentLayer As Integer = g_MapWin.Layers.CurrentLayer
                            Dim lTileGroupHandle As Integer = FindTileGroupHandle()
                            If lTileGroupHandle < 0 Then
                                lTileGroupHandle = g_MapWin.Layers.Groups.Add("Tiles", 0)
                                g_MapWin.Layers.Groups.ItemByHandle(lTileGroupHandle).Expanded = False
                            End If
                            Dim layerCount As Integer = g_MapWin.Layers.Groups.ItemByHandle(lTileGroupHandle).LayerCount
                            ' Paul Meems: Set the sampling methods:
                            'lImage.DownsamplingMode = MapWinGIS.tkInterpolationMode.imBilinear
                            'lImage.UpsamplingMode = MapWinGIS.tkInterpolationMode.imBilinear

                            ' Paul Meems: Using the new interface to directly set the group and position:
                            Dim lNewLayer As MapWindow.Interfaces.Layer = g_MapWin.Layers.Add(lImage, "mwTile-" & lLocalFilename.Replace(pProjectTileCacheFolder, ""), True, lTileGroupHandle, layerCount)
                            With lNewLayer
                                .SkipOverDuringSave = True
                                .HideFromLegend = True
                                'TODO Use Dyn visibility:
                                '.UseDynamicVisibility = True
                                'If aZoom >= g_TileServer.ZoomMax Then
                                '    .MinVisibleScale = 0
                                'Else
                                '    .MinVisibleScale = GetZoomScale(aZoom + 1).Scale
                                'End If

                                'If aZoom <= g_TileServer.ZoomMin Then
                                '    .MaxVisibleScale = 10000000000
                                'Else
                                '    .MaxVisibleScale = GetZoomScale(aZoom - 1).Scale
                                'End If

                            End With
                            If lCurrentLayer >= 0 Then g_MapWin.Layers.CurrentLayer = lCurrentLayer 'Make sure we don't change selected layer to a tile
                        Catch e As Exception
                            Dbg("Could not add to map: " & aFilename & vbLf & e.Message & vbLf & e.StackTrace)
                        Finally
                            g_MapWin.View.UnlockLegend()
                            pTileLayersMutex.ReleaseMutex()
                        End Try
                        Return True
                    Else
                        Debug.WriteLine("Unable to open " & lLocalFilename)
                    End If
                End If
            End If
        Catch e As Exception
            Dbg("Unable to open " & aFilename & vbLf & e.Message & vbLf & e.StackTrace)
        End Try
        Return False
    End Function

    Private Sub MoveLayerToTop(ByVal aGroupHandle As Integer, ByRef aLayer As MapWindow.Interfaces.Layer)
        Debug.WriteLine("In MoveLayerToTop")
        ' Paul Meems: Moved to a seperate sub
        If aGroupHandle < 0 Then
            Debug.WriteLine("Warning in MoveLayerToTop! lTileGroupHandle: " & aGroupHandle)
            Return
        End If

        If aLayer Is Nothing Then
            Debug.WriteLine("Warning in MoveLayerToTop! lNewLayer is Nothing")
            Return
        End If

        Dim layerCount As Integer = g_MapWin.Layers.Groups.ItemByHandle(aGroupHandle).LayerCount
        'aLayer.MoveTo(layerCount, aGroupHandle) 'Move newest tile to top of tile group
        aLayer.GroupHandle = aGroupHandle
        aLayer.GroupPosition = layerCount
        aLayer.Visible = True 'Make sure the tile is visible
        Debug.WriteLine("Moved " & aLayer.Name & " to " & layerCount)
    End Sub

    Private Function FindTileGroupHandle() As Integer
        Try
            For lGroupIndex As Integer = 0 To g_MapWin.Layers.Groups.Count - 1
                If g_MapWin.Layers.Groups(lGroupIndex).Text = "Tiles" Then
                    Return g_MapWin.Layers.Groups(lGroupIndex).Handle
                End If
            Next
        Catch
        End Try
        Return -1
    End Function


    Public Function CreateGeoReferenceFile(ByVal aNorthEdge As Double, _
                                           ByVal aWestEdge As Double, _
                                           ByVal aZoom As Integer, _
                                           ByVal aSaveAsFileName As String) As Boolean
        'http://en.wikipedia.org/wiki/World_file
        'http://support.esri.com/index.cfm?fa=knowledgebase.techarticles.articleShow&d=17489
        ' and thanks to Bostjan for the idea

        Dim lFormat As String = "0.00000000000000000"
        Dim lMetersPerPixel As Double = MetersPerPixel(aZoom)
        Dim lFileWriter As New IO.StreamWriter(aSaveAsFileName)
        If lFileWriter IsNot Nothing Then
            With lFileWriter
                .WriteLine(Format(lMetersPerPixel, lFormat)) ' size of pixel in x direction
                .WriteLine(lFormat)
                .WriteLine(lFormat)
                .WriteLine(Format(-lMetersPerPixel, lFormat)) ' size of pixel in y direction (same x to be square, but negative)
                .WriteLine(Format(aWestEdge, lFormat))
                .WriteLine(Format(aNorthEdge, lFormat))
                .Close()
                Return True
            End With
        End If
        Return False
    End Function
    ' Paul Meems Added:
    'Private Sub FillZoomLevelList()
    '    ' From http://wiki.openstreetmap.org/wiki/FAQ#What_is_the_map_scale_for_a_particular_zoom_level_of_the_map.3F
    '    Dim i As Integer = 0

    '    ZoomList.Add(New Zoomlevels(i, 444000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 222000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 111000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 55000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 28000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 14000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 7000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 3000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 2000000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 867000))
    '    i = i + 1
    '    ZoomList.Add(New Zoomlevels(i, 433000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 217000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 108000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 54000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 27000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 14000))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 6771))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 3385))
    '    i = i + 1
    '    If i > g_TileServer.ZoomMax Then Exit Sub
    '    ZoomList.Add(New Zoomlevels(i, 1693))
    'End Sub
    ' Paul Meems Added:
    'Private Function GetZoomLevel(ByVal scale As Double) As Zoomlevels
    '    'Dim zoomlevel As Double = g_TileServer.ZoomMin
    '    Dim retVal As New Zoomlevels
    '    'Default values
    '    retVal.Zoomlevel = g_TileServer.ZoomMin
    '    retVal.Scale = scale
    '    Try
    '        'Dim result As Zoomlevels = ZoomList.FirstOrDefault(Function(elm) elm.Scale = scale)
    '        retVal = (From elm In ZoomList Select elm, nearest = Math.Abs(elm.Scale - scale) Order By nearest).First().elm
    '        'Dim resolution As Double = result.Resolution
    '    Catch ex As Exception

    '    End Try
    '    Return retVal
    'End Function
    ' Paul Meems Added:
    'Private Function GetZoomScale(ByVal zoomlevel As Integer) As Zoomlevels
    '    Dim retVal As New Zoomlevels
    '    'Default values
    '    retVal.Zoomlevel = zoomlevel
    '    retVal.Scale = 0
    '    Try
    '        retVal = (From elm In ZoomList Select elm, nearest = Math.Abs(elm.Zoomlevel - zoomlevel) Order By nearest).First().elm
    '    Catch ex As Exception

    '    End Try
    '    Return retVal
    'End Function

End Class
' Paul Meems Added:
'Public Class Zoomlevels
'    Private m_scale As Double
'    Private m_zoomLevel As Integer

'    'Constructors
'    Public Sub New(ByVal zoomlevel As Integer, ByVal scale As Double)
'        m_scale = scale
'        m_zoomLevel = zoomlevel
'    End Sub
'    Public Sub New()
'    End Sub

'    Public Property Scale() As Double
'        Get
'            Return m_scale
'        End Get
'        Set(ByVal value As Double)
'            m_scale = value
'        End Set
'    End Property

'    Public Property Zoomlevel() As Integer
'        Get
'            Return m_zoomLevel
'        End Get
'        Set(ByVal value As Integer)
'            m_zoomLevel = value
'        End Set
'    End Property
'End Class

