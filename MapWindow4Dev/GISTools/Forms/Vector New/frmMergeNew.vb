'********************************************************************************************************
'File Name: frmMergeNew.vb
'Description: merges 2 shapefiles by copying shapes from both shapefile in the resultant one
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

Public Class frmMergeNew

    ''' <summary>
    ''' Creates a new instance of the frmMergeNew class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer1)
        ShapefileTools.PopulateShapefileList(cboLayer2)
    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer2.SelectedIndexChanged, cboLayer1.SelectedIndexChanged

        Dim combo As ComboBox = cboLayer1
        Dim lbl As Label = lblSelected1
        Dim chk As CheckBox = chkSelectedOnly1

        If CType(sender, ComboBox) Is cboLayer2 Then
            combo = cboLayer2
            lbl = lblSelected2
            chk = chkSelectedOnly2
        End If

        lbl.Text = "Number of selected: 0"

        If (combo.Items.Count <= 0) Then Return
        Dim name As String = combo.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then
            lbl.Text = "Number of selected: " & sf.NumSelected
            chk.Enabled = sf.NumSelected > 0
            If combo Is cboLayer1 Then
                txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, "merge")
            End If
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
        If cboLayer1.Text = String.Empty Then
            Logger.Msg("First layer is not selected")
        ElseIf cboLayer2.Text = String.Empty Then
            Logger.Msg("Second layer is not selected")
        ElseIf txtFilename.Text = String.Empty Then
            Logger.Msg("Output filename is not specified")
        ElseIf File.Exists(txtFilename.Text) Then
            Logger.Msg("The filename selected already exists")
        ElseIf cboLayer1.SelectedIndex = cboLayer2.SelectedIndex Then
            Logger.Msg("Two different shapefiles should be set as input")
        Else
            Dim sf1 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer1.Text)
            Dim sf2 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer2.Text)
            If Not sf1 Is Nothing And Not sf2 Is Nothing Then
                Dim result As MapWinGIS.Shapefile = Nothing
                result = sf1.Merge(chkSelectedOnly1.Checked And chkSelectedOnly1.Enabled, sf2, chkSelectedOnly1.Checked And chkSelectedOnly2.Enabled)
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
                    Logger.Msg("No result was returned" + vbNewLine + sf1.ErrorMsg(sf1.LastErrorCode))
                End If
            End If
        End If
    End Sub
End Class