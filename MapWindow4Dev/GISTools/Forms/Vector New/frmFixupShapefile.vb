'********************************************************************************************************
'File Name: frmFixupShapefile.vb
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
'26-aug-11 - Sergei Leschinski - created original tool
'********************************************************************************************************

Imports MapWindow.Interfaces
Imports System.Windows.Forms
Imports MapWinUtility
Imports System.IO

Public Class frmFixUpShapefile

    ''' <summary>
    ''' Creates a new instance of the frmExportSelected class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer, False, False)

        btnBrowse.Enabled = cboLayer.Items.Count
        btnOk.Enabled = cboLayer.Items.Count
    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged

        lblInvalidCount.Text = "Number of invalid: 0"

        If (cboLayer.Items.Count <= 0) Then Return
        Dim name As String = cboLayer.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then
            Dim invalidCount As Integer = Me.GetInvalidShapesCount(sf)
            lblInvalidCount.Text = String.Format("Invalid shapes: {0}", invalidCount)
            If invalidCount > 0 Then
                txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, "fixed")
            Else
                txtFilename.Text = ""
            End If
            btnOk.Enabled = invalidCount > 0
        End If
    End Sub

    ''' <summary>
    ''' Chooses the file to save the results to
    ''' </summary>
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ShapefileTools.ChooseShapefileName(txtFilename)
    End Sub

    ''' <summary>
    ''' Runs the tool
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
                Dim invalidCount As Integer = Me.GetInvalidShapesCount(sf)

                Dim sfResult As MapWinGIS.Shapefile = Nothing
                Dim result As Boolean = sf.FixUpShapes(sfResult)

                ' ask the user if the partial result is ok
                If Not result AndAlso Not sfResult Is Nothing Then
                    Dim countNew As Integer = Me.GetInvalidShapesCount(sfResult)
                    If countNew < invalidCount Then
                        If MessageBox.Show(String.Format("Only {0} from {1} invalid shapes were fixed. Do you want to save the result al the same?", _
                                                         invalidCount - countNew, invalidCount), g_MW.ApplicationInfo.ApplicationName, _
                                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            result = True
                        Else
                            sfResult.Close()
                        End If
                    End If
                End If

                If result Then
                    If Not sfResult.SaveAs(txtFilename.Text, Nothing) Then
                        Logger.Msg(sfResult.ErrorMsg(sfResult.LastErrorCode))
                    End If
                    sfResult.Close()
                    If MsgBox("Dow you want to add the new layer to the map?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim newLayer As Layer = g_MW.Layers.Add(txtFilename.Text)
                    End If
                Else
                    MessageBox.Show("None of the invalid shapes was fixed.", g_MW.ApplicationInfo.ApplicationName, _
                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.Close()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns the number of invalid shapes
    ''' </summary>
    Private Function GetInvalidShapesCount(ByVal sf As MapWinGIS.Shapefile) As Integer
        Dim count As Integer = 0
        Try
            Cursor = Cursors.WaitCursor
            For i As Integer = 0 To sf.NumShapes - 1
                If Not sf.Shape(i).IsValid Then count = count + 1
            Next
            Return count
        Finally
            Cursor = Cursors.Default
        End Try
    End Function

End Class