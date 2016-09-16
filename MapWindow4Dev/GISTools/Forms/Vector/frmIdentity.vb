'********************************************************************************************************
'File Name: frmIdentity.vb
'Description: Tool to compute a geometric intersection of the input shapes and the indentity shapes.
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
'06/24/07 - Simon Batson - created original tool based on Angela's code in frmClipPolyWLine.vb
'08/29/07 - Simon Batson - Removed existing file check for output file
'08/29/08 - Jiri Kadlec - added logging
'********************************************************************************************************
Public Class frmIdentity
    Inherits System.Windows.Forms.Form

    Private lastInput As String
    Private lastIdentity As String


    Public Sub Initialize()
        stsBar.Text = "Select the input shapefile and the polygon to identity with."
        PopulateShapefileComboBoxes()
        setDefaultColoring()
    End Sub

    Private Sub setDefaultColoring()
        lblInput.ForeColor = Drawing.Color.Black
        lblIdentity.ForeColor = Drawing.Color.Black
        txtbxOutFile.ForeColor = Drawing.Color.Black
    End Sub

    Private Sub PopulateShapefileComboBoxes()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        ' The input shapefile can be Point, Line or Polygon
        ' The identity shapefile can only be Polygon
        cmbxInput.Items.Clear()
        cmbxIdentity.Items.Clear()

        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxIdentity.Items.Add(newlayer.Name)
                    cmbxInput.Items.Add(newlayer.Name)
                ElseIf newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Then
                    cmbxInput.Items.Add(newlayer.Name)
                ElseIf newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Then
                    cmbxInput.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxIdentity.Items.Count > 0) Then
            If lastIdentity <> "" Then
                cmbxIdentity.SelectedIndex = cmbxIdentity.Items.IndexOf(lastIdentity)
            Else
                cmbxIdentity.SelectedIndex = 0
            End If
        End If

        If (cmbxInput.Items.Count > 0) Then
            If lastInput <> "" Then
                cmbxInput.SelectedIndex = cmbxInput.Items.IndexOf(lastInput)
            Else
                cmbxInput.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim okToStart As Boolean
        okToStart = False
        setDefaultColoring()

        Try
            If (cmbxInput.Items.Count > 0) Then
                okToStart = True
            Else
                stsBar.Text = "Please select an input shapefile."
                lblInput.ForeColor = Drawing.Color.Red
                cmbxInput.Focus()
                okToStart = False
            End If

            If (okToStart) Then
                If (cmbxIdentity.Items.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select an identity shapefile."
                    lblIdentity.ForeColor = Drawing.Color.Red
                    cmbxIdentity.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If (txtbxOutFile.Text <> "") Then
                    okToStart = True
                Else
                    stsBar.Text = "Please specify an output file."
                    lblSaveAs.ForeColor = Drawing.Color.Red
                    txtbxOutFile.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If System.IO.File.Exists(txtbxOutFile.Text) Then
                    'make sure that the file is not already in use
                    Dim inUse As Boolean
                    inUse = False
                    Dim i As Integer
                    For i = 0 To g_MW.Layers.NumLayers - 1
                        If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = txtbxOutFile.Text Then
                            inUse = True
                            i = g_MW.Layers.NumLayers 'break from loop
                        End If
                    Next
                    If inUse = True Then
                        'Do not continue until user has changed the file name
                        System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", "File In Use", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                        okToStart = False
                        txtbxOutFile.Focus()
                    End If
                End If 'end of checking if outFile exists and is in use
            End If 'end of checking if all variables are valid
        Catch ex As Exception
        End Try

        If (okToStart) Then
            Cursor = Windows.Forms.Cursors.WaitCursor
            stsBar.Text = "Running Identity Process"
            '8/29/2008 Jiri Kadlec - added logging
            MapWinUtility.Logger.Dbg("GISTools-Identity: Started running Identity Process")
            Identity()
            MapWinUtility.Logger.Dbg("GISTools-Identity: Finished running Identity Process")
            Cursor = Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnBrowseInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseInput.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINT Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTZ Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ _
                    Then
                    AddMap(strPath, 1)
                    PopulateShapefileComboBoxes()
                    lastInput = getNameByPath(strPath)
                    cmbxInput.SelectedIndex = cmbxInput.Items.IndexOf(lastInput)
                Else
                    MsgBox("Please select a shapefile.")
                End If
                sf.Close()
            Else
                MsgBox("That layer already exists. Please select it from the drop down list.")
            End If
        End If

    End Sub
    Private Sub btnBrowseIdentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseIdentity.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ _
                Then
                    AddMap(strPath, 1)
                    PopulateShapefileComboBoxes()
                    lastIdentity = getNameByPath(strPath)
                    cmbxIdentity.SelectedIndex = cmbxIdentity.Items.IndexOf(lastIdentity)
                Else
                    MsgBox("Please select a POLYGON shapefile.")
                End If
                sf.Close()
            Else
                MsgBox("That layer already exists. Please select it from the drop down list.")
            End If
        End If

    End Sub
    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim enableOK As Boolean
        enableOK = True
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagSave As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
        fdiagSave.Filter = sf.CdlgFilter
        fdiagSave.FilterIndex = 1
        fdiagSave.CheckFileExists = False
        If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagSave.FileName
            If Not layerExists(strPath) Then
                txtbxOutFile.Text = strPath
                enableOK = True
                btnOK.Enabled = True
            Else
                MsgBox("This layer already exists. Please remove it or select a different file to save to.")
                enableOK = False
                btnOK.Enabled = False
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If

    End Sub

    Private Function layerExists(ByVal strPath As String) As Boolean
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub AddMap(ByVal fname As String, ByVal addType As Integer)
        Dim FileDesc As String
        FileDesc = System.IO.Path.GetFileName(fname)
        If (FileDesc = "sta.adf") Then
            FileDesc = System.IO.Path.GetDirectoryName(fname)
            FileDesc = System.IO.Path.GetFileNameWithoutExtension(FileDesc)
        End If

        Dim NewLayer As MapWindow.Interfaces.Layer
        Dim CS As New MapWinGIS.GridColorScheme

        If System.IO.File.Exists(fname) Then
            NewLayer = g_MW.Layers.Add(fname, FileDesc)
            If NewLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                Dim g As MapWinGIS.Grid = NewLayer.GetGridObject
                NewLayer.ColoringScheme = CS
                CS.UsePredefined(g.Minimum, g.Maximum, MapWinGIS.PredefinedColorScheme.SummerMountains)
                g_MW.Layers.RebuildGridLayer(NewLayer.Handle, NewLayer.GetGridObject, CS)
            Else
                If g_MW.Layers.NumLayers >= 2 Then
                    NewLayer.OutlineColor = System.Drawing.Color.Blue
                Else
                    NewLayer.OutlineColor = System.Drawing.Color.Red
                End If
                NewLayer.LineOrPointSize = 2
                NewLayer.DrawFill = True
            End If
        End If
    End Sub

    Private Function getNameByPath(ByVal strPath As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name
            End If
        Next
        Return ""
    End Function

    Private Function getPathByName(ByVal strName As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name = strName Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName
            End If
        Next
        Return ""
    End Function


    Private Sub Identity()
        ' Get full paths for selected files
        Dim inputFile As String = getPathByName(cmbxInput.Items(cmbxInput.SelectedIndex))
        Dim identityFile As String = getPathByName(cmbxIdentity.Items(cmbxIdentity.SelectedIndex))
        Dim resultFile As String = txtbxOutFile.Text
        ' Check files selected
        If inputFile.Length = 0 Then
            stsBar.Text = "Please select an input shapefile."
            lblInput.ForeColor = Drawing.Color.Red
            cmbxInput.Focus()
        End If
        If identityFile.Length = 0 Then
            stsBar.Text = "Please select an identity shapefile."
            lblIdentity.ForeColor = Drawing.Color.Red
            cmbxIdentity.Focus()
        End If
        If resultFile.Length = 0 Then
            stsBar.Text = "Please select an output shapefile."
            lblSaveAs.ForeColor = Drawing.Color.Red
            txtbxOutFile.Focus()
        End If
        ' Run Identity process

        'delete file if it already exists
        DeleteShapefile(resultFile)

        If inputFile.Length > 0 And _
        identityFile.Length > 0 And _
        resultFile.Length > 0 Then

            '8/29/2008 Jiri Kadlec - put it in a try/catch block to prevent MapWindow from crashing
            Try
                If MapWinGeoProc.SpatialOperations.Identity(inputFile, identityFile, resultFile) Then
                    ' Optionally add results to map
                    If Me.chkbxAddClip.Checked Then
                        AddMap(resultFile, 1)
                    End If
                    stsBar.Text = "Identity processing finished."
                Else
                    stsBar.Text = "Identity processing failed."
                End If
            Catch ex As Exception
                stsBar.Text = "Identity processing failed."
                MapWinUtility.Logger.Dbg("Identity processing failed. " & ex.Message & ex.StackTrace)
            End Try

        End If

    End Sub

    Private Sub DeleteShapefile(ByRef sfPath As String)
        If System.IO.File.Exists(sfPath) Then
            Dim length As Integer
            length = sfPath.IndexOf(".", 0)
            Dim path As String
            path = sfPath.Substring(0, length)
            System.IO.File.Delete(path & ".shp")
            System.IO.File.Delete(path & ".shx")
            System.IO.File.Delete(path & ".dbf")
        End If
    End Sub

End Class