'********************************************************************************************************
'File Name: frmCentroids.vb
'Description: Creates centroids for polygon shapefiles
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
'10-aug-11 - Paul Meems - created original tool
'********************************************************************************************************

Imports System.Collections.Generic
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO

Public Class frmCentroids


    ''' <summary>
    ''' Creates a new instance of the frmExplode class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer, eLayerType.PolygonShapefile)
    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged

        If (cboLayer.Items.Count <= 0) Then Return

        Dim name As String = cboLayer.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then
            txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, "centroids")
            ' Fill the listbox with the fields:
            FieldsListbox.Items.Clear()
            For i As Integer = 0 To sf.NumFields - 1
                FieldsListbox.Items.Add(sf.Field(i).Name)
            Next i
        End If
    End Sub

    ''' <summary>
    ''' Chooses the file to save the results to
    ''' </summary>
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ShapefileTools.ChooseShapefileName(txtFilename)
    End Sub

    ''' <summary>
    ''' Runs the centroid routine
    ''' </summary>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        If cboLayer.Text = String.Empty Then
            Logger.Msg("Layer is not selected")
        ElseIf txtFilename.Text = String.Empty Then
            Logger.Msg("Output filename is not specified")
        ElseIf File.Exists(txtFilename.Text) Then
            Logger.Msg("The filename selected already exists")
        Else
            Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer.Text)

            If Not sf Is Nothing Then

                Dim result As MapWinGIS.Shapefile = MapWinGeoProc.SpatialOperations.CreateCentroids(sf, Nothing, chechAllParts.Checked, radioCenter.Checked)

                If Not result Is Nothing Then
                    ' Add additional fields:
                    AddFieldsAndData(sf, result)

                    If Not result.SaveAs(txtFilename.Text, Nothing) Then
                        Logger.Msg(result.ErrorMsg(result.LastErrorCode))
                    End If
                    result.Close()
                    If checkAddToMap.Checked Then
                        g_MW.Layers.Add(txtFilename.Text)
                    End If
                    Me.Close()
                Else
                    Logger.Msg("No result was returned" + vbNewLine + sf.ErrorMsg(sf.LastErrorCode))
                End If
        End If
        End If
    End Sub

    Private Sub AddFieldsAndData(ByRef sf As MapWinGIS.Shapefile, ByRef result As MapWinGIS.Shapefile)
        If FieldsListbox.CheckedItems.Count = 0 Then Return

        ' Get the field index of the parentID field, this field is filled while adding the 
        ' centroid shapes and contains the shapeID of the shape the centroid is made for:
        Dim parentIDFieldIndex As Integer = result.Table().FieldIndexByName("ParentID")
        If parentIDFieldIndex < 0 Then Return

        ' Add every checked field:
        For Each itemChecked As Object In FieldsListbox.CheckedItems
            Dim fieldName As String = itemChecked.ToString()
            'Get the field from the input shapefile:
            Dim newField As MapWinGIS.Field = sf.FieldByName(fieldName)

            If Not newField Is Nothing Then
                'Get the index of the field from the input shapefile""
                Dim dataFieldIndex As Integer = sf.Table().FieldIndexByName(fieldName)
                'Get the index of the field we're going to add:
                Dim fieldIndex As Integer = result.NumFields
                ' Add the new field:
                If (result.EditInsertField(newField, fieldIndex)) Then
                    ' Loop though all centroids:
                    For i As Integer = 0 To result.NumShapes - 1
                        ' Get the parentID from the centroid shapefile for each shape:
                        Dim parentID As Integer = result.CellValue(parentIDFieldIndex, i)
                        'Get the cell value of the field from the input file using the parentID:
                        Dim cellValue As Object = sf.CellValue(dataFieldIndex, parentID)
                        ' Add data to the centroid shapefile
                        result.EditCellValue(fieldIndex, i, cellValue)
                    Next i
                End If
            End If
        Next
    End Sub
End Class