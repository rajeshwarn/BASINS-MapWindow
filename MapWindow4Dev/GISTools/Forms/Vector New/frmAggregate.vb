'********************************************************************************************************
'File Name: frmAggregate.vb
'Description: Tool for aggregating shapes by attribute (makes multi-part shapes from single part ones)
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

Public Class frmAggregate

    ''' <summary>
    ''' Creates a new instance of the frmAggregate class
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

        cboField.Items.Clear()
        lblSelected.Text = "Number of selected: 0"

        If (cboLayer.Items.Count <= 0) Then Return
        Dim name As String = cboLayer.Items(cboLayer.SelectedIndex)

        ShapefileTools.PopulateFieldList(name, cboField)

        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then
            If cboField.Items.Count > 0 Then cboField.SelectedIndex = 0
            lblSelected.Text = "Number of selected: " & sf.NumSelected
            chkSelectedOnly.Enabled = sf.NumSelected > 0
            txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, "aggr")
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
        If cboLayer.Text = String.Empty Then
            Logger.Msg("Layer is not selected")
        ElseIf cboField.Text = String.Empty And optAggregateByField.Checked Then
            Logger.Msg("Field is not selected")
        ElseIf txtFilename.Text = String.Empty Then
            Logger.Msg("Output filename is not specified")
        ElseIf File.Exists(txtFilename.Text) Then
            Logger.Msg("The filename selected already exists")
        Else
            Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer.Text)
            If Not sf Is Nothing Then

                Dim index As Integer = IIf(optAggregateAll.Checked, -1, cboField.SelectedIndex)
                Dim result As MapWinGIS.Shapefile
                result = sf.AggregateShapes(chkSelectedOnly.Checked And chkSelectedOnly.Enabled, index)

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

    ''' <summary>
    ''' Toggles the enabled state of field combo
    ''' </summary>
    Private Sub optAggregateAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAggregateAll.CheckedChanged, optAggregateByField.CheckedChanged
        cboField.Enabled = optAggregateByField.Checked
    End Sub

End Class