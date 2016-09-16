Imports MapWinUtility

'********************************************************************************************************
'File Name: frmDissolve.vb
'Description: Tool for merging shapes by attribute
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
'13-mar-11 - Sergei Leschinski - created original tool
'********************************************************************************************************

Public Class frmSpatialQuery

    ''' <summary>
    ''' Creates a new instance of the frmDissilve class
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        cboRelation.Items.Clear()
        cboRelation.Items.Add("Contain")
        cboRelation.Items.Add("Cross")
        cboRelation.Items.Add("Disjoint from")
        cboRelation.Items.Add("Equal")
        cboRelation.Items.Add("Intersect")
        cboRelation.Items.Add("Overlap")
        cboRelation.Items.Add("Touch")
        cboRelation.Items.Add("Within")
        cboRelation.SelectedIndex = 0

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboSubject)
        ShapefileTools.PopulateShapefileList(cboSelection)
    End Sub

    ''' <summary>
    ''' Shows the number of selected shapes for the definition layer
    ''' </summary>
    Private Sub cboSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSelection.SelectedIndexChanged
        lblSelected.Text = "Number of selected: 0"
        Dim layerName As String = cboSelection.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(layerName)
        If Not sf Is Nothing Then
            lblSelected.Text = "Number of selected: " & sf.NumSelected
            chkSelectedOnly.Enabled = sf.NumSelected > 0
        End If
    End Sub

    ''' <summary>
    ''' Start the selection process
    ''' </summary>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If cboSubject.Text = String.Empty Then
            Logger.Msg("Subject layer is not selected")
        ElseIf cboSelection.Text = String.Empty Then
            Logger.Msg("Definition layer is not selected")
            'ElseIf cboSubject.Text = cboSelection.Text Then
            '    Logger.Msg("Subject and definition layers are the same")
        Else
            Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboSubject.Text)
            Dim sfDefinition As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboSelection.Text)

            If Not sf Is Nothing And Not sfDefinition Is Nothing Then
                Dim arr As Object = Nothing
                If sf.SelectByShapefile(sfDefinition, CType(cboRelation.SelectedIndex, MapWinGIS.tkSpatialRelation), _
                                        chkSelectedOnly.Checked And chkSelectedOnly.Enabled, arr) Then
                    If arr.Length > 0 Then

                        Dim options As ArrayList = New ArrayList()
                        options.Add("1 - New selection")
                        options.Add("2 - Add to selection")
                        options.Add("3 - Exclude from selection")
                        options.Add("4 - Invert in selection")
                        Dim s As String = String.Format("Number of shapes = {0}. Choose the way to update selection", arr.Length)
                        Dim choice As Integer = MapWindow.Controls.Dialogs.ChooseOptions(options, 0, s, "Update selection")

                        ' TODO: at modes for updating selection
                        Dim layer As MapWindow.Interfaces.Layer = ShapefileTools.LayerByName(cboSubject.Text)
                        If Not layer Is Nothing And choice <> -1 Then
                            g_MW.View.UpdateSelection(layer.Handle, arr, CType(choice, MapWindow.Interfaces.SelectionOperation))
                        End If
                        g_MW.View.Redraw()
                    End If
                End If
            End If
        End If
    End Sub
End Class