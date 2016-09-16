'********************************************************************************************************
'File Name: frmExplode.vb
'Description: creates single part shapes from the multi-part ones
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
'15-mar-11 - Sergei Leschinski - created original tool
'********************************************************************************************************

Imports MapWindow.Interfaces
Imports System.Windows.Forms
Imports MapWinUtility
Imports System.IO

Public Class frmSimplifyLines

    Private m_units As MapWindow.Interfaces.UnitOfMeasure = UnitOfMeasure.Unknown

    ''' <summary>
    ''' Creates a new instance of the frmExplode class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer)

    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged

        lblSelected.Text = "Number of selected: 0"

        If (cboLayer.Items.Count <= 0) Then Return
        Dim name As String = cboLayer.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then

            Dim prj As String = sf.GeoProjection.ExportToProj4()
            If prj <> "" Then
                Dim units As MapWindow.Interfaces.UnitOfMeasure = MapWinGeoProc.UnitConverter.GetShapefileUnits(prj)
                cboUnits.DataSource = [Enum].GetNames(GetType(MapWindow.Interfaces.UnitOfMeasure))
                If m_units = UnitOfMeasure.Unknown Then
                    cboUnits.SelectedIndex = Convert.ToInt32(units)
                Else
                    cboUnits.SelectedIndex = Convert.ToInt32(m_units)
                End If
            Else
                cboUnits.Items.Add("Map units")
                cboUnits.SelectedIndex = 0
            End If

            lblSelected.Text = "Number of selected: " & sf.NumSelected
            chkSelectedOnly.Enabled = sf.NumSelected > 0
            txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, "simplify")
        End If
    End Sub

    ''' <summary>
    ''' Chooses the file to save the results to
    ''' </summary>
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ShapefileTools.ChooseShapefileName(txtFilename)
    End Sub

    ''' <summary>
    ''' Runs th dissolve routine
    ''' </summary>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim tolerance As Double
        If Not Double.TryParse(txtDistance.Text, tolerance) Then
            Logger.Msg("Invalid value of the tolerance")
        ElseIf cboLayer.Text = String.Empty Then
            Logger.Msg("Layer is not selected")
        ElseIf txtFilename.Text = String.Empty Then
            Logger.Msg("Output filename is not specified")
        ElseIf File.Exists(txtFilename.Text) Then
            Logger.Msg("The filename selected already exists")
        Else
            Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer.Text)

            If Not sf Is Nothing Then
                ' if the list of units was provided
                If cboUnits.Items.Count > 1 Then
                    Dim target As MapWindow.Interfaces.UnitOfMeasure = MapWinGeoProc.UnitConverter.GetShapefileUnits(sf.GeoProjection.ExportToProj4())
                    Dim source As MapWindow.Interfaces.UnitOfMeasure = CType(cboUnits.SelectedIndex, MapWindow.Interfaces.UnitOfMeasure)
                    If target = UnitOfMeasure.Unknown Then target = source
                    If source <> target Then
                        tolerance = (MapWinGeoProc.UnitConverter.ConvertLength(source, target, tolerance))
                    End If
                End If

                Dim result As MapWinGIS.Shapefile = sf.SimplifyLines(tolerance, chkSelectedOnly.Checked And chkSelectedOnly.Enabled)
                If Not result Is Nothing Then
                    If Not result.SaveAs(txtFilename.Text, Nothing) Then
                        Logger.Msg(result.ErrorMsg(result.LastErrorCode))
                    End If
                    result.Close()
                    If MsgBox("Dow you want to add the new layer to the map?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim newLayer As Layer = g_MW.Layers.Add(txtFilename.Text)
                    End If
                    Me.Close()
                Else
                    Logger.Msg("No result was returned" + vbNewLine + sf.ErrorMsg(sf.LastErrorCode))
                End If
            End If
        End If
    End Sub
End Class