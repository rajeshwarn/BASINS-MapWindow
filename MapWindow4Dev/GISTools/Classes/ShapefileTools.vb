Imports System.IO
Imports System.Windows.Forms
Imports MapWinUtility
Imports MapWindow.Interfaces

Public Class ShapefileTools

#Region "NewGeoprocessing"

    ''' <summary>
    ''' Fills the list of shapefile layers
    ''' </summary>
    Public Shared Sub PopulateShapefileList(ByRef combo As ComboBox, Optional ByVal SelectedOnly As Boolean = False, Optional ByVal SelectFirst As Boolean = True)

        combo.Items.Clear()
        For i As Integer = 0 To g_MW.Layers.NumLayers - 1
            Dim layer As Layer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
            If layer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Or _
               layer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Or _
               layer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Then

                If SelectedOnly Then
                    Dim sf As MapWinGIS.Shapefile = CType(layer.GetObject(), MapWinGIS.Shapefile)
                    If Not sf Is Nothing Then
                        If sf.NumSelected > 0 Then
                            combo.Items.Add(layer.Name)
                        End If
                    End If
                Else
                    combo.Items.Add(layer.Name)
                End If
            End If
        Next
        If Not combo Is Nothing AndAlso combo.Items.Count > 0 And SelectFirst Then
            combo.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Fills the list of shapefile layers of a specific type
    ''' </summary>
    Public Shared Sub PopulateShapefileList(ByRef combo As ComboBox, ByVal layerType As MapWindow.Interfaces.eLayerType, Optional ByVal SelectedOnly As Boolean = False)

        combo.Items.Clear()
        For i As Integer = 0 To g_MW.Layers.NumLayers - 1
            Dim layer As Layer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
            If layer.LayerType = layerType Then

                If SelectedOnly Then
                    Dim sf As MapWinGIS.Shapefile = CType(layer.GetObject(), MapWinGIS.Shapefile)
                    If Not sf Is Nothing Then
                        If sf.NumSelected > 0 Then
                            combo.Items.Add(layer.Name)
                        End If
                    End If
                Else
                    combo.Items.Add(layer.Name)
                End If
            End If
        Next
        If Not combo Is Nothing AndAlso combo.Items.Count > 0 Then
            combo.SelectedIndex = 0
        End If

    End Sub
    ''' <summary>
    ''' Fills the list of fields for the shapefile
    ''' </summary>
    Public Shared Sub PopulateFieldList(ByVal layerName As String, ByRef combo As ComboBox)
        combo.Items.Clear()
        Dim layer As Layer = LayerByName(layerName)
        If Not layer Is Nothing Then
            Dim sf As MapWinGIS.Shapefile = CType(layer.GetObject(), MapWinGIS.Shapefile)
            If Not sf Is Nothing Then
                For j As Integer = 0 To sf.NumFields - 1
                    combo.Items.Add(sf.Field(j).Name)
                Next j
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns reference to the layer with the specified name
    ''' </summary>
    Public Shared Function LayerByName(ByVal name As String) As Layer
        For i As Integer = 0 To g_MW.Layers.NumLayers - 1
            Dim layer As Layer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
            If layer.Name = name Then
                Return layer
            End If
        Next i
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns shapefile associated with the layer name
    ''' </summary>
    Public Shared Function ShapefileByLayerName(ByVal name As String) As MapWinGIS.Shapefile
        Dim layer As Layer = LayerByName(name)
        If Not layer Is Nothing Then
            Return CType(layer.GetObject(), MapWinGIS.Shapefile)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Sub ChooseShapefileName(ByVal textBox As TextBox)
        Dim sf As New MapWinGIS.Shapefile
        Dim dlg As SaveFileDialog = New SaveFileDialog
        dlg.Filter = sf.CdlgFilter
        dlg.FilterIndex = 1

        dlg.FileName = textBox.Text

        If dlg.ShowDialog() = DialogResult.OK Then
            If File.Exists(dlg.FileName) Then
                Logger.Msg("Layer <" & dlg.FileName & "> already exists.", MsgBoxStyle.Exclamation, "GISTools:Disolve:FileExists")
            Else
                textBox.Text = dlg.FileName
            End If
        End If

        dlg.Dispose()
    End Sub

    ''' <summary>
    ''' Returns the available filename with the specified postfix.
    ''' Applicable for shapefiles only.
    ''' </summary>
    Public Shared Function GetAvailibleFileName(ByVal filename As String, ByVal postfix As String) As String

        Dim dir As String = Path.GetDirectoryName(filename)
        Dim name As String = Path.GetFileNameWithoutExtension(filename)
        Dim ext As String = Path.GetExtension(filename)
        Dim i As Integer = 0
        Dim shp As String, shx As String, dbf As String, prj As String
        Do
            i = i + 1
            shp = dir & "\" & name & "_" & postfix & CStr(i) & ext
            shx = dir & "\" & name & "_" & postfix & CStr(i) & ".shx"
            dbf = dir & "\" & name & "_" & postfix & CStr(i) & ".dbf"
            prj = dir & "\" & name & "_" & postfix & CStr(i) & ".prj"
        Loop While File.Exists(shp) OrElse File.Exists(shx) OrElse File.Exists(dbf) OrElse FileExists(prj)
        Return shp

    End Function
#End Region

#Region "Methods"
    ''' <summary>
    ''' DoExportSelected                                              
    ''' Exports the selected MapWindow shapes to a new shapefile.
    ''' </summary>
    ''' <remarks>
    ''' By Chris Michaelis Aug 2006
    ''' Change Log: 
    ''' Date          Changed By      Notes
    ''' 08/22/2006    JLK             MsgBox to Logger.Msg, added additional Logger messages
    ''' </remarks>
    Friend Sub DoExportSelected()
        Dim lDisplayName As String = "GIS Tools:Export Selected:"
        If Globals.g_MW.Layers.NumLayers = 0 Then
            Logger.Msg("No layers available to export selected shapes from.", _
                       MsgBoxStyle.Exclamation, _
                       lDisplayName & "No Layers")
            Exit Sub
        End If
        If Globals.g_MW.View.SelectedShapes.NumSelected = 0 Then
            Logger.Msg("There are no selected features to export! Please select a feature first.", _
                       MsgBoxStyle.Exclamation, _
                       lDisplayName & "Empty Selection")
            Exit Sub
        End If

        Dim saveFileDialog1 As New System.Windows.Forms.SaveFileDialog
        saveFileDialog1.Filter = "Shapefiles (*.shp)|*.shp"
        saveFileDialog1.FilterIndex = 2
        saveFileDialog1.RestoreDirectory = True
        saveFileDialog1.Title = lDisplayName & "Save File Name"
        If saveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim loadshapefilechoice As Boolean = _
              IIf(MapWinUtility.Logger.Msg("Do you want to load the new shapefile?", _
                  MsgBoxStyle.YesNo, lDisplayName & "Load Layer") _
                  = MsgBoxResult.Yes, True, False)
            Logger.Dbg(lDisplayName & "Start Export of " & _
                       Globals.g_MW.View.SelectedShapes.NumSelected & _
                       " shapes from file <" & Globals.g_MW.Layers(Globals.g_MW.Layers.CurrentLayer).FileName & _
                       "> to file <" & saveFileDialog1.FileName & ">")
            MapWinGeoProc.Selection.ExportSelectedMWViewShapes( _
              Globals.g_MW, _
              saveFileDialog1.FileName, _
              loadshapefilechoice)
            Logger.Dbg(lDisplayName & "Done")
        Else
            Logger.Dbg(lDisplayName & "User Canceled Save File Dialog")
            Exit Sub
        End If
    End Sub
#End Region

End Class
