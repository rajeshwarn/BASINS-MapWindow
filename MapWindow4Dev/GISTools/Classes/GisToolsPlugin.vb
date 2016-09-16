Imports System.Drawing
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWindow.Controls.GisToolbox
Imports MapWindow.Controls.Data

''' <summary>
''' Class implementing IPlugin interface for GIS tools plugin
''' </summary>
Public Class GisToolsPlugin
    Implements MapWindow.Interfaces.IPlugin

    ' list of menu items
    Private m_MenuItems As ArrayList = Nothing

    ' reference to resource manager
    Private res As System.Resources.ResourceManager = Nothing

    ' prevents duplication of menu
    Private m_MenusExist As Boolean = False

    ' refernce to the gis toolbox
    Private m_toolbox As IGisToolBox = Nothing

#Region "Plugin properties"
    ''' <summary>
    ''' Gets information about plugin author
    ''' </summary>
    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "MapWindow Open Source Team"
        End Get
    End Property

    ''' <summary>
    ''' Gets information aboub build date
    ''' </summary>
    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location).ToString()
        End Get
    End Property

    ''' <summary>
    ''' Gets plugin descriptions
    ''' </summary>
    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "Generic vector and raster tools for MapWindow."
        End Get
    End Property

    ''' <summary>
    ''' Gets plugin version number
    ''' </summary>
    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Dim major As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString()
            Dim minor As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString()
            Dim build As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString()

            Return major + "." + minor + "." + build
        End Get
    End Property

    ''' <summary>
    ''' Gets plugin name
    ''' </summary>
    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "GIS Tools"
        End Get
    End Property

    ''' <summary>
    ''' Gets plugin serial number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return ""
        End Get
    End Property
#End Region

#Region "Initialization"
    ''' <summary>
    ''' Creates new instance of GISTools plugin class
    ''' </summary>
    Public Sub New()
        res = New System.Resources.ResourceManager("GISTools.Resource", System.Reflection.Assembly.GetExecutingAssembly())
    End Sub

    ''' <summary>
    ''' Initializes GIS tools plugin
    ''' </summary>
    ''' <param name="MapWin">Reference to MapWindow</param>
    ''' <param name="ParentHandle">The handle of parent window</param>
    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        g_MW = MapWin
        g_MapWindowForm = System.Windows.Forms.Control.FromHandle(New IntPtr(ParentHandle))
        m_toolbox = g_MW.GisToolbox

        ' lsu 07-july-2011: commented, tools will avialble in treeview
        'g_MWMenus.LoadMenus()

        Me.InitToolbox()
    End Sub

    ''' <summary>
    ''' Initilizes geoprocessing toobox. Icons are specified inside Mapwindow
    ''' </summary>
    Public Sub InitToolbox()
        Dim group As IGisToolboxGroup = Nothing

        ' projections
        Dim groupProjections As IGisToolboxGroup = m_toolbox.CreateGroup("Projections", "Provides tools for view, assign and change projections for layers.")
        group = Me.CreateGroup(VECTOR_PROJECTIONS)
        Me.AddTool(SHAPEFILE_ASSIGN_PROJECTION, groupProjections)
        Me.AddTool(SHAPEFILE_PROJECTION_IDENTIFY, groupProjections)
        Me.AddTool(SHAPEFILE_PROJECTION_MANAGER, groupProjections)
        Me.AddTool(SHAPEFILE_REPROJECT, groupProjections)
        groupProjections.Expanded = True
        m_toolbox.Groups.Add(groupProjections)

        Dim groupVector As IGisToolboxGroup = m_toolbox.CreateGroup("Vector Operations", "Provides tools for processing ESRI shapefile layers.")

        ' standard
        group = Me.CreateGroup(VECTOR_STANDARD)
        Me.AddTool(SHAPEFILE_3D_TO_2D, group)
        Me.AddTool(SHAPEFILE_AGGREGATE, group)
        Me.AddTool(SHAPEFILE_BUFFER_NEW, group)
        Me.AddTool(SHAPEFILE_CALCULATE_AREA, group)
        Me.AddTool(SHAPEFILE_CENTROIDS, group)
        Me.AddTool(SHAPEFILE_DISSOLVE, group)
        Me.AddTool(SHAPEFILE_EXPLODE, group)
        Me.AddTool(SHAPEFILE_EXPORT_SELECTED, group)
        Me.AddTool(SHAPEFILE_FIXUP, group)
        Me.AddTool(SHAPEFILE_MERGE_NEW, group)
        Me.AddTool(SHAPEFILE_SIMPLIFY_LINES, group)
        Me.AddTool(SHAPEFILE_SORT, group)
        Me.AddTool(SHAPEFILE_SPATIAL_QUERY, group)
        Me.AddTool(SHAPEFILE_VALIDATE, group)
        group.Expanded = True
        groupVector.SubGroups.Add(group)

        ' overlay
        group = Me.CreateGroup(VECTOR_OVERLAYS)
        Me.AddTool(SHAPEFILE_OVERLAY_CLIP, group)
        Me.AddTool(SHAPEFILE_OVERLAY_DIFFERENCE, group)
        Me.AddTool(SHAPEFILE_OVERLAY_INTERSECTION, group)
        Me.AddTool(SHAPEFILE_OVERLAY_SYM_DIFFERENCE, group)
        Me.AddTool(SHAPEFILE_OVERLAY_UNION, group)
        group.Expanded = False
        groupVector.SubGroups.Add(group)

        ' old tools
        group = Me.CreateGroup(VECTOR_OLD_TOOLS)
        Me.AddTool(SHAPEFILE_BUFFER, group)
        Me.AddTool(SHAPEFILE_CLIP_POLY_WITH_LINE, group)
        Me.AddTool(SHAPEFILE_CLIP_WITH_POLYGON, group)
        Me.AddTool(SHAPEFILE_ERASE_WITH_POLYGON, group)
        Me.AddTool(SHAPEFILE_EXPORT_BY_MASK, group)
        Me.AddTool(SHAPEFILE_MERGE, group)
        Me.AddTool(SHAPEFILE_MERGE_SF, group)
        group.Expanded = False

        groupVector.SubGroups.Add(group)

        m_toolbox.Groups.Add(groupVector)
        groupVector.Expanded = True

        ' database tools
        group = Me.CreateGroup(DATABASE_TOOLS)
        Me.AddTool(DATABASE_SAVE_TABLE, group)
        'Me.AddTool(DATABASE_LOAD_TABLE, group)

        m_toolbox.Groups.Add(group)
        group.Expanded = True

        ' image operations
        group = Me.CreateGroup(IMAGE_OPERATIONS)
        Me.AddTool(IMAGE_ASSIGN_PROJECTION, group)
        Me.AddTool(IMAGE_REPROJECT, group)
        Me.AddTool(IMAGE_RECTIFY_WORLD_FILE, group)
        group.Expanded = True
        m_toolbox.Groups.Add(group)

        ' raster operation
        group = Me.CreateGroup(RASTER_OPERATIONS)
        Me.AddTool(GRID_ASSIGN_PROJECTION, group)
        Me.AddTool(GRID_REPROJECT, group)
        Me.AddTool(GRID_CHANGE_FORMAT, group)
        Me.AddTool(GRID_CREATE_IMAGE, group)
        Me.AddTool(GRID_RESAMPLE, group)
        Me.AddTool(GRID_MERGE, group)
        Me.AddTool(GRID_CLIP_WITH_POLY, group)
        Me.AddTool(GRID_GEOREFERENCE, group)
        Me.AddTool(GRID_CONTOUR, group)
        Me.AddTool(GRID_CHANGE_NO_DATA, group)
        group.Expanded = True
        m_toolbox.Groups.Add(group)

        'm_toolbox.ExpandGroups(2)

        m_MenusExist = True

        AddHandler m_toolbox.ToolClicked, AddressOf ToolClickedHandler
    End Sub
#End Region

    ''' <summary>
    ''' Adds new tool to the toolbox
    ''' </summary>
    Private Sub AddTool(ByVal toolName As String, ByVal group As IGisToolboxGroup)
        Dim tool As IGisTool = m_toolbox.CreateTool(res.GetString("mnu" + toolName.Substring(8) + ".Text"), toolName)
        tool.Description = Globals.GetDescription(toolName)
        group.Tools.Add(tool)
    End Sub

    ''' <summary>
    ''' Creates new group
    ''' </summary>
    Private Function CreateGroup(ByVal groupName As String) As IGisToolboxGroup
        Dim group As IGisToolboxGroup = m_toolbox.CreateGroup(res.GetString(groupName), groupName)
        group.Description = Globals.GetGroupDescription(groupName)
        Return group
    End Function


#Region "Termination"
    ''' <summary>
    ''' Destroys the menus
    ''' </summary>
    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate

        ' removing all the nodes ( more intelligent will be needed in case GIS tools won't be an application plug-in)
        m_toolbox.Groups.Clear()

        'If Not m_MenusExist Then Return
        'If g_MW Is Nothing Then Return

        'Dim i As IEnumerator = m_MenuItems.GetEnumerator()
        'While (i.MoveNext())
        '    If CType(i.Current, MapWindow.Interfaces.MenuItem).Name = "" Then
        '        g_MW.Menus.Remove(CType(i.Current, MapWindow.Interfaces.MenuItem).Text)
        '    Else
        '        g_MW.Menus.Remove(CType(i.Current, MapWindow.Interfaces.MenuItem).Name)
        '    End If
        'End While

        'm_MenusExist = False
        'm_MenuItems.Clear()
    End Sub
#End Region

#Region "Event Handling"
    ''' <summary>
    ''' Handles mouse down event for image georeferencing form
    ''' </summary>
    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
        If Not g_georef_form Is Nothing AndAlso Not g_georef_form.IsDisposed Then
            Dim pX As Double = 0
            Dim pY As Double = 0
            g_MW.View.PixelToProj(x, y, pX, pY)
            g_georef_form.NotifyMapClick(pX, pY)
        End If
    End Sub

    ''' <summary>
    '''  Handles tool clicked event and runs appropriate tools
    ''' </summary>
    ''' <param name="tool">The tool that was clicked</param>
    ''' <param name="handled">Notifies the caller that event was handled</param>
    Private Sub ToolClickedHandler(ByVal tool As IGisTool, ByRef handled As Boolean)
        If Not m_MenusExist Then Return

        handled = True
        Select Case tool.Key

            Case "mwTools_MapWindowTools"
                'Do nothing; placeholder.
            Case "mwTools_Raster"
                'Do nothing; placeholder.
            Case "mwTools_Vector"
                'Do nothing; placeholder.
            Case "mwTools_Image"
                'Do nothing; placeholder.
            Case "mwTools_VectorOverlay"
                'Do nothing; placeholder.
            Case "mwTools_VectorNew"
                'Do nothing; placeholder.


            Case DATABASE_SAVE_TABLE
                Dim form As New frmExportShapefile(g_MW)
                form.ShowDialog(g_MW)
                form.Dispose()
            Case SHAPEFILE_FIXUP
                Dim form As New frmFixUpShapefile()
                form.ShowDialog(g_MW)
                form.Dispose()

            Case SHAPEFILE_VALIDATE
                Dim form As New frmValidate()
                form.ShowDialog(g_MW)
                form.Dispose()

            Case SHAPEFILE_CENTROIDS
                Dim form As New frmCentroids()
                form.ShowDialog(g_MW)
                form.Dispose()

            Case SHAPEFILE_PROJECTION_IDENTIFY

                Dim layer As MapWindow.Interfaces.Layer = g_MW.Layers(g_MW.Layers.CurrentLayer)
                Dim bounds As MapWinGIS.Extents = Nothing
                If Not layer Is Nothing Then bounds = layer.Extents
                Dim form As New MapWindow.Controls.Projections.frmIdentifyProjection(g_MW, bounds)
                If Not layer Is Nothing Then form.textBox1.Text = layer.Projection

                If form.ShowDialog(g_MW) = DialogResult.OK Then
                    ' do something
                End If
                form.Dispose()

            Case SHAPEFILE_SIMPLIFY_LINES
                Dim form As New frmSimplifyLines
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_EXPORT_SELECTED
                'DoExportSelected()
                Dim form As New frmExportSelected
                form.ShowDialog()
                form.Close()

            Case SHAPEFILE_EXPORT_BY_MASK
                If g_exportbymask_form Is Nothing OrElse g_exportbymask_form.IsDisposed Then g_exportbymask_form = New frmExportByMask()
                g_exportbymask_form.Initialize()
                g_exportbymask_form.Show()

            Case SHAPEFILE_PROJECTION_MANAGER
                Dim form As New MapWindow.Controls.Projections.frmProjectionManager(g_MW)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_ASSIGN_PROJECTION
                ProjectionTools.DoApplySFProj(g_MW)

            Case GRID_CHANGE_NO_DATA
                Dim ChangeNodataDlg As New frmChangeNodataVal
                ChangeNodataDlg.ShowDialog()

            Case SHAPEFILE_REPROJECT
                'ProjectionTools.DoSFReproject(g_MW)
                Dim form As New MapWindow.Controls.Projections.frmReproject(g_MW)
                If form.ShowDialog(g_MW) = DialogResult.OK Then
                    ' do sonething
                End If
                form.Dispose()
            Case GRID_ASSIGN_PROJECTION
                GridTools.DoApplyGridProj(g_MW)

            Case GRID_REPROJECT
                GridTools.DoGridReproject(g_MW)

            Case GRID_CHANGE_FORMAT
                GridTools.mnuChangeGridFormats()

            Case GRID_CREATE_IMAGE
                GridTools.mnuCreateGridImages()

            Case GRID_MERGE
                GridTools.mnuMergeGrids()

            Case GRID_RESAMPLE
                GridTools.mnuResampleGrids()

            Case GRID_CLIP_WITH_POLY
                Dim frmClipping As New frmClip
                frmClipping.Initialize(0)
                frmClipping.Show()

            Case SHAPEFILE_BUFFER_NEW
                Dim bufferForm As New frmBufferNew()
                bufferForm.ShowDialog()

            Case SHAPEFILE_BUFFER
                Dim bufferForm As New frmBuffer()
                bufferForm.Initialize()
                bufferForm.Show()

            Case SHAPEFILE_CLIP_WITH_POLYGON
                Dim frmClipping As New frmClip
                frmClipping.Initialize(1)
                frmClipping.Show()

            Case SHAPEFILE_CLIP_POLY_WITH_LINE
                Dim frmClipping As New frmClipPolyWLine
                frmClipping.Initialize()
                frmClipping.Show()

            Case SHAPEFILE_ERASE_WITH_POLYGON
                Dim eraseForm As New frmErase()
                eraseForm.Initialize()
                eraseForm.Show()

            Case SHAPEFILE_CALCULATE_AREA
                Dim calcfrm As New frmCalculateArea(g_MW)
                If Not calcfrm Is Nothing AndAlso Not calcfrm.IsDisposed Then
                    calcfrm.Show()
                End If

            Case SHAPEFILE_3D_TO_2D
                Dim converterFrm As New frm3Dto2D()
                converterFrm.Show()

            Case GRID_CONTOUR
                Dim calcfrm As New frmGenerateContour(g_MW)
                calcfrm.Show()

            Case SHAPEFILE_MERGE
                Dim frmMergeShapes As New frmMerge
                frmMergeShapes.Initialize()
                frmMergeShapes.Show()

            Case SHAPEFILE_MERGE_SF
                Dim mrg As New frmMergeShapefiles(g_MW)
                mrg.ShowDialog()

            Case SHAPEFILE_IDENTITY
                Dim frmIdentity As New frmIdentity
                frmIdentity.Initialize()
                frmIdentity.Show()

            Case GRID_GEOREFERENCE
                If g_georef_form Is Nothing OrElse _
                   g_georef_form.IsDisposed Then
                    g_georef_form = New frmGeoreference()
                End If
                g_georef_form.Show()

            Case IMAGE_ASSIGN_PROJECTION
                ImageTools.DoApplyImageProj(g_MW)

            Case IMAGE_RECTIFY_WORLD_FILE
                ImageTools.DoRectifyToWorldfile(g_MW)

            Case IMAGE_REPROJECT
                ImageTools.DoImageReproject(g_MW)

            Case SHAPEFILE_DISSOLVE
                Dim form As New frmDissolve
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_OVERLAY_INTERSECTION
                Dim form As New frmClipOperation(MapWinGIS.tkClipOperation.clIntersection)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_OVERLAY_DIFFERENCE
                Dim form As New frmClipOperation(MapWinGIS.tkClipOperation.clDifference)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_OVERLAY_UNION
                Dim form As New frmClipOperation(MapWinGIS.tkClipOperation.clUnion)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_OVERLAY_SYM_DIFFERENCE
                Dim form As New frmClipOperation(MapWinGIS.tkClipOperation.clSymDifference)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_OVERLAY_CLIP
                Dim form As New frmClipOperation(MapWinGIS.tkClipOperation.clClip)
                form.ShowDialog(g_MapWindowForm)
                form.Dispose()

            Case SHAPEFILE_SPATIAL_QUERY
                Dim form As New frmSpatialQuery()
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_EXPLODE
                Dim form As New frmExplode()
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_AGGREGATE
                Dim form As New frmAggregate()
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_SORT
                Dim form As New frmSortShapefile()
                form.ShowDialog()
                form.Dispose()

            Case SHAPEFILE_MERGE_NEW
                Dim form As New frmMergeNew
                form.ShowDialog()
                form.Dispose()

            Case Else
                handled = False
        End Select
    End Sub
#End Region

#Region "Unimplmented members of IPlugin interface"

    Public Sub ItemClicked(ByVal itemName As String, ByRef handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked

    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved

    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded

    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared

    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected

    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick

    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown

    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp

    End Sub

    Public Sub MapDragFinished(ByVal Bounds As Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished

    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged

    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove

    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp

    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message

    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading

    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving

    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected

    End Sub
#End Region

#Region "Load menus(obsolete)"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: LoadMenus
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to load GIS Tool Menu items
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header and added ClipMerge submenu
    ' 11/21/2005    Chris Michaelis Added Georeferencing Stuff
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub LoadMenus()
        If m_MenusExist Then Return
        If g_MW Is Nothing Then Return

        If (m_MenuItems Is Nothing) Then
            m_MenuItems = New ArrayList
        Else
            m_MenuItems.Clear()
        End If

        m_MenusExist = True

        ' Here, use a very unique phrase to avoid duplication: mwTools_MapWindowTools
        ' Each subitem will begin with "mwTools_" by convention
        Dim rootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_MapWindowTools")
        If (rootItem Is Nothing) Then rootItem = g_MW.Menus.AddMenu("mwTools_MapWindowTools", "", Nothing, res.GetString("mnuGISTools.Text"))
        m_MenuItems.Add(rootItem)

        Dim rasterRootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_Raster")
        If (rasterRootItem Is Nothing) Then rasterRootItem = g_MW.Menus.AddMenu("mwTools_Raster", rootItem.Name, Nothing, res.GetString("mnuRaster.Text"))
        m_MenuItems.Add(rasterRootItem)

        Dim vectorRootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_Vector")
        If (vectorRootItem Is Nothing) Then vectorRootItem = g_MW.Menus.AddMenu("mwTools_Vector", rootItem.Name, Nothing, res.GetString("mnuVector.Text"))
        m_MenuItems.Add(vectorRootItem)

        Dim imageRootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_Image")
        If (imageRootItem Is Nothing) Then imageRootItem = g_MW.Menus.AddMenu("mwTools_Image", rootItem.Name, Nothing, res.GetString("mnuImage.Text"))
        m_MenuItems.Add(imageRootItem)

        Dim overlaysRootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_VectorOverlay")
        If (overlaysRootItem Is Nothing) Then overlaysRootItem = g_MW.Menus.AddMenu("mwTools_VectorOverlay", rootItem.Name, Nothing, res.GetString("mnuVectorOverlays.Text"))
        m_MenuItems.Add(overlaysRootItem)

        Dim vectorNewRootItem As MapWindow.Interfaces.MenuItem = g_MW.Menus.Item("mwTools_VectorNew")
        If (vectorNewRootItem Is Nothing) Then vectorNewRootItem = g_MW.Menus.AddMenu("mwTools_VectorNew", rootItem.Name, Nothing, res.GetString("mnuVectorNew.Text"))
        m_MenuItems.Add(vectorNewRootItem)

        'Vector:
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ApplyProjSF", vectorRootItem.Name, Nothing, res.GetString("mnuApplyProjSF.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ReProjSF", vectorRootItem.Name, Nothing, res.GetString("mnuReProjSF.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_CalculateArea", vectorRootItem.Name, Nothing, res.GetString("mnuCalculateArea.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ClipPolyWithLine", vectorRootItem.Name, Nothing, res.GetString("mnuClipPolyWithLine.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ClipSFWithPoly", vectorRootItem.Name, Nothing, res.GetString("mnuClipSFWithPoly.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_EraseSFWithPoly", vectorRootItem.Name, Nothing, res.GetString("mnuEraseSFWithPoly.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ExportByMask", vectorRootItem.Name, Nothing, res.GetString("mnuExportByMask.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Merge", vectorRootItem.Name, Nothing, res.GetString("mnuMerge.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_MergeSF", vectorRootItem.Name, Nothing, res.GetString("mnuMergeSF.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Identity", vectorRootItem.Name, Nothing, res.GetString("mnuIdentity.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_XYZtoXY", vectorRootItem.Name, Nothing, res.GetString("mnu3Dto2D.Text")))

        ' New vector
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Aggregate", vectorNewRootItem.Name, Nothing, res.GetString("mnuAggregateShapes.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Buffer", vectorNewRootItem.Name, Nothing, res.GetString("mnuBuffer.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Dissolve", vectorNewRootItem.Name, Nothing, res.GetString("mnuDissolveByAttribute.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Explode", vectorNewRootItem.Name, Nothing, res.GetString("mnuExplodeShapes.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ExportSelected", vectorNewRootItem.Name, Nothing, res.GetString("mnuExportSelected.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_MergeShapefiles", vectorNewRootItem.Name, Nothing, res.GetString("mnuMergeNew.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_SortShapefile", vectorNewRootItem.Name, Nothing, res.GetString("mnuSortShapefile.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_SpatialQuery", vectorNewRootItem.Name, Nothing, res.GetString("mnuSpatialQuery.Text")))

        ' Vector overlays
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Clip", overlaysRootItem.Name, Nothing, res.GetString("mnuSfClip.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Difference", overlaysRootItem.Name, Nothing, res.GetString("mnuSfDifference.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Intersection", overlaysRootItem.Name, Nothing, res.GetString("mnuSfIntersection.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_SymmDifference", overlaysRootItem.Name, Nothing, res.GetString("mnuSfSymmDifference.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Union", overlaysRootItem.Name, Nothing, res.GetString("mnuSfUnion.Text")))

        'Raster:
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ApplyProjGRID", rasterRootItem.Name, Nothing, res.GetString("mnuApplyProjGRID.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ReProjGRID", rasterRootItem.Name, Nothing, res.GetString("mnuReProjGRID.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ChangeFormatGRID", rasterRootItem.Name, Nothing, res.GetString("mnuChangeFormatGRID.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_CreateImageGRID", rasterRootItem.Name, Nothing, res.GetString("mnuCreateImageGRID.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ResampleGRID", rasterRootItem.Name, Nothing, res.GetString("mnuResampleGRID.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_MergeGRIDS", rasterRootItem.Name, Nothing, res.GetString("mnuMergeGRIDS.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ClipGridWithPoly", rasterRootItem.Name, Nothing, res.GetString("mnuClipGridWithPoly.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Georeference", rasterRootItem.Name, Nothing, res.GetString("mnuGeoreference.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_Contour", rasterRootItem.Name, Nothing, res.GetString("mnuContour.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ChangeNodata", rasterRootItem.Name, Nothing, res.GetString("mnuChangeNodata.Text")))

        'Image:
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ApplyProjImage", imageRootItem.Name, Nothing, res.GetString("mnuApplyProjImage.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_RectifyToWorldfile", imageRootItem.Name, Nothing, res.GetString("mnuRectifyToWorldfile.Text")))
        m_MenuItems.Add(g_MW.Menus.AddMenu("mwTools_ReProjImage", imageRootItem.Name, Nothing, res.GetString("mnuReProjImage.Text")))
    End Sub
#End Region

End Class
