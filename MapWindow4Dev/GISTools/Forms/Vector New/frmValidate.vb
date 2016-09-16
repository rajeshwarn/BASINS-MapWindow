'********************************************************************************************************
'File Name: frmValidate.vb
'Description: exports selected shapes to the new shapefile
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source GIS Tools Plug-in. 
'
'Contributor(s): (Open source contributors should list themselves and their modifications here).
'06 aug 11 - Sergei Leschinski - created original tool
'********************************************************************************************************

Imports MapWindow.Interfaces
Imports System.Windows.Forms
Imports System.Linq
Imports System.Collections.Generic
Imports MapWinUtility
Imports System.IO

Public Class frmValidate

#Region "Declrations"
    ' Possible GEOS errors
    Private m_errors() As String = {"Topology Validation Error", _
                                  "Repeated Point", _
                                  "Hole lies outside shell", _
                                  "Holes are nested", _
                                  "Interior is disconnected", _
                                  "Ring Self-intersection", _
                                  "Self-intersection", _
                                  "Nested shells", _
                                  "Duplicate Rings", _
                                  "Too few points in geometry component", _
                                  "Invalid Coordinate", _
                                  "Ring is not closed"}

    ' point shapefile with position of errors
    Private m_points As MapWinGIS.Shapefile = Nothing

    ' a structire to hold information about errors in the listbox
    Private Structure ErrorInfo
        Dim Message As String
        Dim ShapeIndex As Integer

        Public Overrides Function ToString() As String
            Return Message.ToString()
        End Function
    End Structure

    ' the handle of the point layer added to the map
    Private m_layerHandle As Integer = -1
#End Region

    ''' <summary>
    ''' Creates a new instance of the frmExportSelected class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.lblCount.Text = ""

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer, False)

        btnOk.Enabled = cboLayer.Items.Count
    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged

        If (cboLayer.Items.Count <= 0) Then Return
        Dim name As String = cboLayer.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
    End Sub

    ''' <summary>
    ''' Runs the tool
    ''' </summary>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Me.ListBox1.Items.Clear()
        m_layerHandle = -1
        lblCount.Text = ""


        If cboLayer.Text = String.Empty Then
            Logger.Msg("Layer is not selected")
            Return
        End If

        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer.Text)
        If Not sf Is Nothing Then
            Dim shp As MapWinGIS.Shape
            Dim message As String = ""
            Dim count As Integer = 0

            Try
                btnOk.Enabled = False
                'g_MW.View.LockMap()
                sf.SelectNone()
                Dim percent As Integer = 0, newPercent As Integer

                For i As Integer = 0 To sf.NumShapes - 1
                    newPercent = i / (sf.NumShapes - 1) * 100
                    If newPercent > percent Then
                        CType(g_MW.Layers, MapWinGIS.ICallback).Progress("", percent, "Validating...")
                        percent = newPercent
                    End If

                    shp = sf.Shape(i)
                    If Not shp Is Nothing AndAlso Not shp.IsValid Then
                        Dim reason As String = shp.IsValidReason
                        Me.AddError(reason, sf, i)
                        message += String.Format("ID={0}: {1}" + Environment.NewLine, i, reason)
                        count = count + 1
                    End If
                Next
            Finally
                CType(g_MW.Layers, MapWinGIS.ICallback).Progress("", 100, "")
                'g_MW.View.UnlockMap()
                btnOk.Enabled = True
            End Try

            Dim result As String = ""
            If count = 0 Then
                result = "All shapes are valid"
            Else
                result = "Invalid shapes were found: " + count.ToString()
                If Not m_points Is Nothing AndAlso m_points.NumShapes > 0 Then
                    If m_points.StopEditingShapes(True, True, Nothing) Then
                        Dim layer As MapWindow.Interfaces.Layer = ShapefileTools.LayerByName(cboLayer.Text)
                        m_layerHandle = layer.Handle
                    End If
                Else
                    If Not m_points Is Nothing Then m_points.Close()
                End If
            End If

            m_points = Nothing
            g_MW.View.Redraw()
            g_MW.View.UnlockMap()
            Application.DoEvents()

            MessageBox.Show(result, g_MW.ApplicationInfo.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Logger.Msg("An error while obtaining the shapefile" + vbNewLine + sf.ErrorMsg(sf.LastErrorCode))
        End If
    End Sub

#Region "Point shapefile"
    ''' <summary>
    ''' Creates point shapefile woth errors
    ''' </summary>
    Private Sub CreateShapefile(ByVal source As MapWinGIS.Shapefile)
        If m_points Is Nothing Then
            Dim filename As String = ShapefileTools.GetAvailibleFileName(source.Filename, "errors")
            m_points = New MapWinGIS.Shapefile
            m_points.CreateNew(filename, MapWinGIS.ShpfileType.SHP_POINT)
            Dim field As New MapWinGIS.Field
            field.Name = "ErrorType"
            field.Type = MapWinGIS.FieldType.INTEGER_FIELD
            m_points.EditInsertField(field, 0, Nothing)
            field = New MapWinGIS.Field()
            field.Name = "ErrorMsg"
            field.Type = MapWinGIS.FieldType.STRING_FIELD
            field.Width = 30
            m_points.EditInsertField(field, 1, Nothing)
            m_points.GeoProjection = g_MW.Project.GeoProjection

            Dim layerNew As MapWindow.Interfaces.Layer = g_MW.Layers.Add(m_points, "Errors - " + cboLayer.Text)

            ' applying color scheme
            Dim legendFilename As String = Path.GetDirectoryName(Application.ExecutablePath) + "\Styles\ShapefileErrors.mwleg"
            If File.Exists(legendFilename) Then
                If Not layerNew Is Nothing Then layerNew.LoadShapefileCategories(legendFilename)
            End If

            g_MW.View.ShowLegend()
        End If
    End Sub

    ''' <summary>
    ''' Adds an error to the list
    ''' </summary>
    ''' <param name="message">Message returned by Shape.IsValid</param>
    ''' <remarks></remarks>
    Private Sub AddError(ByVal message As String, ByVal sfSource As MapWinGIS.Shapefile, ByVal shapeIndex As Integer)

        Dim needRedraw As Boolean = False
        Try
            If Not g_MW.View.IsMapLocked() Then g_MW.View.LockMap()
            sfSource.ShapeSelected(shapeIndex) = True

            Dim info As ErrorInfo
            info.ShapeIndex = shapeIndex
            info.Message = String.Format("ID={0}: {1}" + Environment.NewLine, shapeIndex, message)
            ListBox1.Items.Add(info)
            lblCount.Text = "Invalid shapes: " + ListBox1.Items.Count.ToString()

            ' is it a default error ?
            For i As Integer = 0 To m_errors.Length - 1
                Dim s As String = m_errors(i)
                If message.ToLower().Contains(s.ToLower()) Then

                    Dim pnt As MapWinGIS.Point = Me.GetErrorPoint(message)
                    If Not pnt Is Nothing Then
                        If m_points Is Nothing Then Me.CreateShapefile(sfSource)

                        Dim shp As New MapWinGIS.Shape
                        shp.Create(MapWinGIS.ShpfileType.SHP_POINT)
                        shp.InsertPoint(pnt, 0)
                        m_points.EditInsertShape(shp, m_points.NumShapes)

                        If message.IndexOf("[") > 0 Then
                            m_points.EditCellValue(0, m_points.NumShapes - 1, i)
                            m_points.EditCellValue(1, m_points.NumShapes - 1, message.Substring(0, message.IndexOf("[")))
                        End If

                        m_points.Categories.ApplyExpressions()
                        needRedraw = True
                    End If
                    Exit For
                End If
            Next
        Finally
            If needRedraw Then
                If chkUpdate.Checked Then
                    g_MW.View.Redraw()
                    g_MW.View.UnlockMap()
                End If
                Application.DoEvents()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Extracts coordinates; format = message[X Y]
    ''' </summary>
    ''' <param name="message"></param>
    Private Function GetErrorPoint(ByVal message As String) As MapWinGIS.Point

        Dim index As Integer = message.IndexOf("[")
        If index >= 0 AndAlso index < message.Length - 1 Then
            Dim s As String = message.Substring(index + 1)

            If s.Count > 1 Then
                s = s.Substring(0, s.Length - 1)
                Dim pair() As String = s.Split(" ")
                If pair.Length = 2 Then
                    Dim x As Double, y As Double
                    If Double.TryParse(pair(0), Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, x) AndAlso _
                       Double.TryParse(pair(1), Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, y) Then
                        Dim pnt As New MapWinGIS.Point
                        pnt.x = x
                        pnt.y = y
                        Return pnt
                    End If
                End If
            End If
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Zooms map to the selected shape
    ''' </summary>
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If Not ListBox1.SelectedItem Is Nothing Then
            Dim info As ErrorInfo = CType(ListBox1.SelectedItem, ErrorInfo)
            Dim map As AxMapWinGIS.AxMap = TryCast(g_MW.GetOCX, AxMapWinGIS.AxMap)
            If Not map Is Nothing AndAlso m_layerHandle <> -1 Then
                map.ZoomToShape(m_layerHandle, info.ShapeIndex)
                map.Redraw()
            End If
        End If
    End Sub
#End Region

End Class