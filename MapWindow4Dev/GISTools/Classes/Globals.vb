Imports System.Drawing
Imports System.Xml
Imports MapWinUtility
Imports MapWindow.Interfaces
Imports System.Windows.Forms
Imports System.IO

Module Globals

#Region "Command names"
    ' groups
    Public Const VECTOR_OPERATIONS As String = "mnuVector.Text"
    Public Const VECTOR_PROJECTIONS As String = "mnuVectorProjections.Text"
    Public Const VECTOR_OVERLAYS As String = "mnuVectorOverlays.Text"
    Public Const VECTOR_GENERALIZE As String = "mnuVectorGeneralize.Text"
    Public Const VECTOR_OLD_TOOLS As String = "mnuVectorOld.Text"
    Public Const RASTER_OPERATIONS As String = "mnuRaster.Text"
    Public Const IMAGE_OPERATIONS As String = "mnuImage.Text"
    Public Const VECTOR_STANDARD As String = "mnuVectorStandard.Text"
    Public Const DATABASE_TOOLS As String = "mnuDatabaseTools.Text"

    'Public Const VECTOR_QUERY As String = "mnuVectorQuery.Text"
    'Public Const VECTOR_MODIFY As String = "mnuVectorModify.Text"
    'Public Const VECTOR_CALCULATE As String = "mnuVectorCalculate.Text"
    'Public Const VECTOR_GENERATE As String = "mnuVectorGenerate.Text"

    ' old vector
    Public Const SHAPEFILE_ASSIGN_PROJECTION As String = "mwTools_ApplyProjSF"
    Public Const SHAPEFILE_REPROJECT As String = "mwTools_ReProjSF"
    Public Const SHAPEFILE_PROJECTION_MANAGER As String = "mwTools_ProjectionManager"
    Public Const SHAPEFILE_PROJECTION_IDENTIFY As String = "mwTools_ProjectionIdentify"
    Public Const SHAPEFILE_CALCULATE_AREA As String = "mwTools_CalculateArea"
    Public Const SHAPEFILE_CLIP_POLY_WITH_LINE As String = "mwTools_ClipPolyWithLine"
    Public Const SHAPEFILE_CLIP_WITH_POLYGON As String = "mwTools_ClipSFWithPoly"
    Public Const SHAPEFILE_ERASE_WITH_POLYGON As String = "mwTools_EraseSFWithPoly"
    Public Const SHAPEFILE_EXPORT_BY_MASK As String = "mwTools_ExportByMask"
    Public Const SHAPEFILE_MERGE As String = "mwTools_Merge"
    Public Const SHAPEFILE_MERGE_SF As String = "mwTools_MergeSF"
    Public Const SHAPEFILE_IDENTITY As String = "mwTools_Identity"
    Public Const SHAPEFILE_3D_TO_2D As String = "mwTools_3Dto2D"

    ' new vector
    Public Const SHAPEFILE_AGGREGATE As String = "mwTools_AggregateShapes"
    Public Const SHAPEFILE_BUFFER_NEW As String = "mwTools_BufferNew"
    Public Const SHAPEFILE_BUFFER As String = "mwTools_Buffer"
    Public Const SHAPEFILE_DISSOLVE As String = "mwTools_DissolveByAttribute"
    Public Const SHAPEFILE_EXPLODE As String = "mwTools_ExplodeShapes"
    Public Const SHAPEFILE_EXPORT_SELECTED As String = "mwTools_ExportSelected"
    Public Const SHAPEFILE_MERGE_NEW As String = "mwTools_MergeNew"
    Public Const SHAPEFILE_SORT As String = "mwTools_SortShapefile"
    Public Const SHAPEFILE_SPATIAL_QUERY As String = "mwTools_SpatialQuery"
    Public Const SHAPEFILE_ATTRIBUTE_QUERY As String = "mwTools_AttributeQuery"
    Public Const SHAPEFILE_SIMPLIFY_LINES As String = "mwTools_SimplifyLines"
    Public Const SHAPEFILE_VALIDATE As String = "mwTools_ShapefileValidate"
    Public Const SHAPEFILE_CENTROIDS As String = "mwTools_ShapefileCentroids"
    Public Const SHAPEFILE_FIXUP As String = "mwTools_FixupShapefile"

    ' Vector overlays
    Public Const SHAPEFILE_OVERLAY_CLIP As String = "mwTools_SfClip"
    Public Const SHAPEFILE_OVERLAY_DIFFERENCE As String = "mwTools_SfDifference"
    Public Const SHAPEFILE_OVERLAY_INTERSECTION As String = "mwTools_SfIntersection"
    Public Const SHAPEFILE_OVERLAY_SYM_DIFFERENCE As String = "mwTools_SfSymmDifference"
    Public Const SHAPEFILE_OVERLAY_UNION As String = "mwTools_SfUnion"

    'Raster:
    Public Const GRID_ASSIGN_PROJECTION As String = "mwTools_ApplyProjGRID"
    Public Const GRID_REPROJECT As String = "mwTools_ReProjGRID"
    Public Const GRID_CHANGE_FORMAT As String = "mwTools_ChangeFormatGRID"
    Public Const GRID_CREATE_IMAGE As String = "mwTools_CreateImageGRID"
    Public Const GRID_RESAMPLE As String = "mwTools_ResampleGRID"
    Public Const GRID_MERGE As String = "mwTools_MergeGRIDS"
    Public Const GRID_CLIP_WITH_POLY As String = "mwTools_ClipGridWithPoly"
    Public Const GRID_GEOREFERENCE As String = "mwTools_Georeference"
    Public Const GRID_CONTOUR As String = "mwTools_Contour"
    Public Const GRID_CHANGE_NO_DATA As String = "mwTools_ChangeNodata"

    ' Image
    Public Const IMAGE_ASSIGN_PROJECTION As String = "mwTools_ApplyProjImage"
    Public Const IMAGE_RECTIFY_WORLD_FILE As String = "mwTools_RectifyToWorldfile"
    Public Const IMAGE_REPROJECT As String = "mwTools_ReProjImage"

    ' Database
    Public Const DATABASE_SAVE_TABLE As String = "mwTools_SaveTable"
    Public Const DATABASE_LOAD_TABLE As String = "mwTools_LoadTable"

#End Region

    ' Function prototype for the ExecutionFinished event
    Public Delegate Sub FinishedDelegate(ByVal success As Boolean, ByVal errorMessage As String)
    Public Delegate Sub BlankDelegate()

    Public g_MW As MapWindow.Interfaces.IMapWin
    Public g_MapWindowForm As System.Windows.Forms.Form
    Public g_Grids As New ArrayList()
    Public g_Images As New ArrayList()
    Public g_NewCellSize As Double
    Public g_Scheme As New MapWinGIS.GridColorScheme
    Public g_newDataType As MapWinGIS.GridDataType
    Public g_newFormat As MapWinGIS.GridFileType
    Public g_newExt As String
    Public g_AddOutputToMW As Boolean
    Public g_OutputPath As String
    Public g_OutputName As String
    Public g_georef_form As New frmGeoreference
    Public g_exportbymask_form As New frmExportByMask()

    Public Sub Cleanup()
        On Error Resume Next
        g_MapWindowForm.BringToFront()
        g_MapWindowForm.Focus()
        g_Grids.Clear()
        g_Grids = Nothing
        g_Images.Clear()
        g_Images = Nothing
        g_NewCellSize = -1
        g_Scheme = Nothing
        g_newDataType = MapWinGIS.GridDataType.UnknownDataType
        g_newFormat = MapWinGIS.GridFileType.InvalidGridFileType
        g_newExt = ""
        g_AddOutputToMW = True
    End Sub

    ''' <summary>
    ''' Returns description for specified group
    ''' </summary>
    Public Function GetGroupDescription(ByVal groupName As String) As String
        Select Case groupName
            Case VECTOR_OPERATIONS
                Return "Provide tools for processing ESRI shapefile layers."
            Case VECTOR_STANDARD
                Return "Commonly used set of tools to manipulate vector layers"
            Case VECTOR_OVERLAYS
                Return "Provide tools for generating of new shapefiles by overlay (spatial combination) of 2 input shapefiles."
            Case VECTOR_PROJECTIONS
                Return "Provide tools to to work with coordinate systems and projections of shapefile (assign or change projection)."
            Case VECTOR_GENERALIZE
                Return "Provide tools to change the level of generalization of data, either to more general by aggregating shapes with" _
                & "equal values of attributes or less general by spliting multipart shapes into single part ones."
            Case VECTOR_OLD_TOOLS
                Return "Stores tools which have more effective analogues in Standard or Overlays section (in term of performance). Can still be useful " _
                & "when newer versions lack some essentinal functionality."
            Case IMAGE_OPERATIONS
                Return "Provide tools for processing image files in various formats."
            Case RASTER_OPERATIONS
                Return "Provide tools for processing grid files (rasters with associated color scheme)."
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Returns description for specified command
    ''' </summary>
    ''' <param name="commandName"></param>
    Public Function GetDescription(ByVal commandName As String) As String
        Select Case commandName
            Case SHAPEFILE_OVERLAY_DIFFERENCE
                Return "Creates a combination of 2 shapefiles. Passes to the resulting shapefile those shapes (or parts of shapes) of subject shapefile that aren't covered by shapes of definition shapefile." & _
                       "Attributes are copied from subject shapefile only."
            Case SHAPEFILE_OVERLAY_INTERSECTION
                Return "Creates new shapefile with results of intersection of 2 shapefiles. Only shapes that belong to both shapefile are passed to the resulting shapefile." & _
                       "Attributes are copied from both input shapfiles."
            Case SHAPEFILE_OVERLAY_CLIP
                Return "Creates a combination of 2 shapefiles. Passes to resulting shapefile those shapes (or parts of shapes) of subject shapefile that are covered by shapes of definition shapefile." & _
                       "Attributes are copied from subject shapefile only."
            Case SHAPEFILE_OVERLAY_SYM_DIFFERENCE
                Return "Creates a combination of 2 shapefiles. Passes to the output shapefile the results 2 mirror difference operations (subject-definition; definition-subject)"
            Case SHAPEFILE_OVERLAY_UNION
                Return "Creates a combination of 2 shapefiles. Passes to the output shapefile the results 2 mirror difference operations (subject-definition; definition-subject) " _
                & "and the results of intersection operation for the same shapefiles."
            Case SHAPEFILE_ASSIGN_PROJECTION
                Return "Assigns cooordinate system and projection to shapefile or group of shapefiles for which the coordinate system isn't defined. " _
                & "The operation doesn't change coordinates but tells the program how they must be interpreted."
            Case SHAPEFILE_REPROJECT
                Return "Changes projection or (and) coordinate system of shapefile. The shapefile must have assigned coordinate system to perfrom this operation. " _
                & "This tool changes the coordinates of particular objects."
            Case SHAPEFILE_CALCULATE_AREA
                Return "Calculates area of polygons in the shapefile and writes the results in attribute table."
            Case SHAPEFILE_3D_TO_2D
                Return "Creates 2D shapefile from 3D shapefile. In the former case each point is reprsented by X,Y pair of coordinates, in the later by X, Y, Z triplet."
            Case SHAPEFILE_AGGREGATE
                Return "Creates multi-part shapes from single-part ones based on the coomon values of specified field."
            Case SHAPEFILE_BUFFER_NEW
                Return "Creates a new shapefile which holds polygon buffer zone around objects of input shapefile."
            Case SHAPEFILE_BUFFER
                Return "Creates a new shapefile which holds polygon buffer zone around objects of input shapefile."
            Case SHAPEFILE_DISSOLVE
                Return "Joins shapes with the same value of the specified attribute. "
            Case SHAPEFILE_EXPLODE
                Return "Creates single-part shapes from the multi-part ones."
            Case SHAPEFILE_EXPORT_SELECTED
                Return "Exports selected shapes to the new shapefile."
            Case SHAPEFILE_MERGE_NEW
                Return "Copies objects from 2 input shapefiles in the resulting shapefile. The shapefiles must have the same type."
            Case SHAPEFILE_SORT
                Return "Sorts the objects in shapefile in ascending or descending order according the value of the specified field in attribute table."
            Case SHAPEFILE_SPATIAL_QUERY
                Return "Selects object in the subject shaefile which have certain relations with object of definition shapefiles. The relations can be: intersection, inclusion, etc."
                ' TODO: describe
            Case SHAPEFILE_CLIP_POLY_WITH_LINE
            Case SHAPEFILE_CLIP_WITH_POLYGON
            Case SHAPEFILE_ERASE_WITH_POLYGON
            Case SHAPEFILE_EXPORT_BY_MASK
            Case SHAPEFILE_MERGE
            Case SHAPEFILE_MERGE_SF
            Case SHAPEFILE_IDENTITY
            Case SHAPEFILE_CENTROIDS
                Return "Create centroids of polygon shapes."
            Case Else
                Return ""
        End Select
        Return ""
    End Function
End Module


