'********************************************************************************************************
'File Name: frmClipPolyWLine.vb
'Description: Tool for clipping a polygons with a line.
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
'12/1/05 - Angela Hillier - created original tool based on Allen's code in frmClip.vb
'03/30/06 - Angela Hillier - added checks to see if output file already exists
'4/6/06 - Angela Hillier - fixed the browse buttons, eliminated duplicate user instructions
'5/22/06 - Angela Hillier - fixed 'ok' button so that it is always enabled and validates each input
'********************************************************************************************************
Public Class frmExportByMask
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblToClip As System.Windows.Forms.Label
    Friend WithEvents cmbxToClip As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseToClip As System.Windows.Forms.Button
    Friend WithEvents lblClipWith As System.Windows.Forms.Label
    Friend WithEvents cmbxClipWith As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseClipWith As System.Windows.Forms.Button
    Friend WithEvents btnSelectLine As System.Windows.Forms.Button
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents chkbxAddClip As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExportByMask))
        Me.lblToClip = New System.Windows.Forms.Label
        Me.cmbxToClip = New System.Windows.Forms.ComboBox
        Me.btnBrowseToClip = New System.Windows.Forms.Button
        Me.lblClipWith = New System.Windows.Forms.Label
        Me.cmbxClipWith = New System.Windows.Forms.ComboBox
        Me.btnBrowseClipWith = New System.Windows.Forms.Button
        Me.btnSelectLine = New System.Windows.Forms.Button
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.chkbxAddClip = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.SuspendLayout()
        '
        'lblToClip
        '
        Me.lblToClip.AccessibleDescription = Nothing
        Me.lblToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.lblToClip, "lblToClip")
        Me.lblToClip.Font = Nothing
        Me.lblToClip.Name = "lblToClip"
        '
        'cmbxToClip
        '
        Me.cmbxToClip.AccessibleDescription = Nothing
        Me.cmbxToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxToClip, "cmbxToClip")
        Me.cmbxToClip.BackgroundImage = Nothing
        Me.cmbxToClip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxToClip.Font = Nothing
        Me.cmbxToClip.Name = "cmbxToClip"
        '
        'btnBrowseToClip
        '
        Me.btnBrowseToClip.AccessibleDescription = Nothing
        Me.btnBrowseToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToClip, "btnBrowseToClip")
        Me.btnBrowseToClip.BackgroundImage = Nothing
        Me.btnBrowseToClip.Font = Nothing
        Me.btnBrowseToClip.Name = "btnBrowseToClip"
        '
        'lblClipWith
        '
        Me.lblClipWith.AccessibleDescription = Nothing
        Me.lblClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.lblClipWith, "lblClipWith")
        Me.lblClipWith.Font = Nothing
        Me.lblClipWith.Name = "lblClipWith"
        '
        'cmbxClipWith
        '
        Me.cmbxClipWith.AccessibleDescription = Nothing
        Me.cmbxClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxClipWith, "cmbxClipWith")
        Me.cmbxClipWith.BackgroundImage = Nothing
        Me.cmbxClipWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxClipWith.Font = Nothing
        Me.cmbxClipWith.Name = "cmbxClipWith"
        '
        'btnBrowseClipWith
        '
        Me.btnBrowseClipWith.AccessibleDescription = Nothing
        Me.btnBrowseClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseClipWith, "btnBrowseClipWith")
        Me.btnBrowseClipWith.BackgroundImage = Nothing
        Me.btnBrowseClipWith.Font = Nothing
        Me.btnBrowseClipWith.Name = "btnBrowseClipWith"
        '
        'btnSelectLine
        '
        Me.btnSelectLine.AccessibleDescription = Nothing
        Me.btnSelectLine.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelectLine, "btnSelectLine")
        Me.btnSelectLine.BackgroundImage = Nothing
        Me.btnSelectLine.Font = Nothing
        Me.btnSelectLine.Name = "btnSelectLine"
        '
        'lblSaveAs
        '
        Me.lblSaveAs.AccessibleDescription = Nothing
        Me.lblSaveAs.AccessibleName = Nothing
        resources.ApplyResources(Me.lblSaveAs, "lblSaveAs")
        Me.lblSaveAs.Font = Nothing
        Me.lblSaveAs.Name = "lblSaveAs"
        '
        'txtbxOutFile
        '
        Me.txtbxOutFile.AccessibleDescription = Nothing
        Me.txtbxOutFile.AccessibleName = Nothing
        resources.ApplyResources(Me.txtbxOutFile, "txtbxOutFile")
        Me.txtbxOutFile.BackgroundImage = Nothing
        Me.txtbxOutFile.Font = Nothing
        Me.txtbxOutFile.Name = "txtbxOutFile"
        '
        'chkbxAddClip
        '
        Me.chkbxAddClip.AccessibleDescription = Nothing
        Me.chkbxAddClip.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxAddClip, "chkbxAddClip")
        Me.chkbxAddClip.BackgroundImage = Nothing
        Me.chkbxAddClip.Checked = True
        Me.chkbxAddClip.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxAddClip.Font = Nothing
        Me.chkbxAddClip.Name = "chkbxAddClip"
        '
        'btnOK
        '
        Me.btnOK.AccessibleDescription = Nothing
        Me.btnOK.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.BackgroundImage = Nothing
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = Nothing
        Me.btnCancel.Name = "btnCancel"
        '
        'btnBrowseToOut
        '
        Me.btnBrowseToOut.AccessibleDescription = Nothing
        Me.btnBrowseToOut.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToOut, "btnBrowseToOut")
        Me.btnBrowseToOut.BackgroundImage = Nothing
        Me.btnBrowseToOut.Font = Nothing
        Me.btnBrowseToOut.Name = "btnBrowseToOut"
        '
        'stsBar
        '
        Me.stsBar.AccessibleDescription = Nothing
        Me.stsBar.AccessibleName = Nothing
        resources.ApplyResources(Me.stsBar, "stsBar")
        Me.stsBar.BackgroundImage = Nothing
        Me.stsBar.Font = Nothing
        Me.stsBar.Name = "stsBar"
        '
        'frmExportByMask
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkbxAddClip)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Controls.Add(Me.btnSelectLine)
        Me.Controls.Add(Me.btnBrowseClipWith)
        Me.Controls.Add(Me.cmbxClipWith)
        Me.Controls.Add(Me.lblClipWith)
        Me.Controls.Add(Me.btnBrowseToClip)
        Me.Controls.Add(Me.cmbxToClip)
        Me.Controls.Add(Me.lblToClip)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExportByMask"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private g_PolySel As Integer
    Private g_SelectingShapes As Boolean
    Private g_LineSel As Integer
    Private lastToClip As String
    Private lastClipWith As String
    Private SelPolyCount As Integer
    Private SelLineCount As Integer
    Private line As Long = -1

    Public Sub Initialize()
        SelPolyCount = 0
        SelLineCount = 0
        stsBar.Text = "Select the polygon to clip and line to clip with."
        PopulatePolyWithLineClip()
        setDefaultColoring()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim okToStart As Boolean
        okToStart = False
        setDefaultColoring()

        Try
            If (cmbxToClip.Items.Count > 0) Then
                okToStart = True
            Else
                stsBar.Text = "Please select a shapefile to clip."
                lblToClip.ForeColor = Drawing.Color.Red
                cmbxToClip.Focus()
                okToStart = False
            End If

            If (okToStart) Then
                If (cmbxClipWith.Items.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a shapefile to clip with."
                    lblClipWith.ForeColor = Drawing.Color.Red
                    cmbxClipWith.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If line = -1 Then
                    stsBar.Text = "Please select a shape to clip with."
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
            stsBar.Text = "Exporting Shapes..."
            ExportByMask()
            Cursor = Windows.Forms.Cursors.Default
        End If
    End Sub

    Private Sub PopulatePolyWithLineClip()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxToClip.Items.Clear()
        cmbxClipWith.Items.Clear()

        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(i)
                'Clip With -- only polygon.
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxClipWith.Items.Add(newlayer.Name)
                End If

                'Clipping -- all allowed.
                cmbxToClip.Items.Add(newlayer.Name)
            Next
        End If

        If (cmbxToClip.Items.Count > 0) Then
            If lastToClip <> "" Then
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
            Else
                cmbxToClip.SelectedIndex = 0
            End If
        End If

        If (cmbxClipWith.Items.Count > 0) Then
            btnSelectLine.Enabled = True
            If lastClipWith <> "" Then
                cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
            Else
                cmbxClipWith.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub setDefaultColoring()
        lblToClip.ForeColor = Drawing.Color.Black
        lblClipWith.ForeColor = Drawing.Color.Black
        txtbxOutFile.ForeColor = Drawing.Color.Black
        btnSelectLine.ForeColor = Drawing.Color.Black
    End Sub

    Private Function getPathByName(ByVal strName As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name = strName Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName
            End If
        Next
        Return ""
    End Function

    Private Function getNameByPath(ByVal strPath As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name
            End If
        Next
        Return ""
    End Function

    Private Function getIndexByName(ByVal strName As String) As Integer
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name = strName Then
                Return g_MW.Layers.GetHandle(i)
            End If
        Next
    End Function

    Private Function layerExists(ByVal strPath As String) As Boolean
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub RemoveLayer(ByVal fname As String)
        Dim LyrHandle As Integer
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            LyrHandle = g_MW.Layers.GetHandle(i)
            If LCase(g_MW.Layers(LyrHandle).FileName) = LCase(fname) Then
                g_MW.Layers.Remove(LyrHandle)
            End If
        Next
    End Sub

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

    Public Function AppendExt(ByVal fname As String, ByVal Ext As String) As String
        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "." Then Exit For
        Next i
        If (i = 0) Then
            AppendExt = fname & Ext
        Else
            AppendExt = Strings.Left(fname, i - 1) & Ext & Mid(fname, i)
        End If
    End Function

    Private Sub cmbxToClip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxToClip.SelectedIndexChanged
        If cmbxToClip.Items.Count > 0 Then
            PopulateOutFile()
            SelPolyCount = 0
            lastToClip = cmbxToClip.SelectedItem().ToString()
        End If
        If (lastClipWith <> "") Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If
        setDefaultColoring()
    End Sub

    Private Sub PopulateOutFile()
        Dim outFile As String
        outFile = ""
        If cmbxToClip.Items.Count > 0 Then
            'set the output path
            outFile = AppendExt(getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex)), "_clip")
            Dim i As Integer
            i = 0
            Dim clipExt As String
            While System.IO.File.Exists(outFile)
                clipExt = "_clip" & i
                outFile = AppendExt(getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex)), clipExt)
                i = i + 1
            End While
            txtbxOutFile.Text = outFile
        Else
            txtbxOutFile.Text = ""
        End If
    End Sub

    Private Sub cmbxClipWith_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxClipWith.SelectedIndexChanged
        If cmbxClipWith.Items.Count > 0 Then
            SelLineCount = 0
            stsBar.Text = "0 shapes selected. Click 'Select Shape' to select."
            btnSelectLine.Enabled = True
            lastClipWith = cmbxClipWith.SelectedItem().ToString()
        Else
            btnSelectLine.Enabled = False
        End If
        If (lastToClip <> "") Then
            cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
        End If
        setDefaultColoring()
    End Sub
    '4/6/05 - Angela Hillier - changed to check if file is valid before allowing user to continue
    Private Sub txtbxOutFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxOutFile.TextChanged
        Dim enableOK As Boolean
        enableOK = False
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtbxOutFile.Text)) Then
            enableOK = True
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
                    btnOK.Enabled = False
                    enableOK = False
                Else
                    'Warn user that this file already exists and will be overwritten
                    System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " already exists." & Environment.NewLine & "Please choose another path if you don't want this file to be overwritten.", "File Already Exists", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                    enableOK = True
                End If
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If
    End Sub
    'have the user select a polygon shapefile
    Private Sub btnBrowseToClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToClip.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                AddMap(strPath, 0)
                PopulatePolyWithLineClip()
                lastToClip = getNameByPath(strPath)
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
                sf.Close()
            Else
                mapwinutility.logger.msg("That layer already exists. Please select it from the drop down list.")
            End If
        End If
        If (lastClipWith <> "") Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If
    End Sub
    'have the user select a line shapefile
    Private Sub btnBrowseClipWith_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseClipWith.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                AddMap(strPath, 1)
                PopulatePolyWithLineClip()
                lastClipWith = getNameByPath(strPath)
                cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
            End If
            sf.Close()
        Else
            mapwinutility.logger.msg("That layer already exists. Please select it from the drop down list.")
        End If

        If (lastToClip <> "") Then
            cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSelectLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLine.Click
        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectLine.Text = "Select Shape"
            SelLineCount = g_MW.View.SelectedShapes.NumSelected
            If SelLineCount = 0 Then
                stsBar.Text = "0 shapes selected. Click the 'Select Shape' button to select."
            ElseIf SelLineCount = 1 Then
                line = (g_MW.View.SelectedShapes.Item(0).ShapeIndex)
                stsBar.Text = SelLineCount.ToString + " shape selected."
            Else
                MapWinUtility.Logger.Msg("Please select only one polygon.")
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Click Done once finished."
            btnSelectLine.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxClipWith.Items(cmbxClipWith.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
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
    Private Function CheckBounds(ByRef shp1 As MapWinGIS.Shape, ByRef shp2 As MapWinGIS.Shape) As Boolean
        Dim overlap As Boolean
        overlap = False
        Dim xMin1, xMax1, yMin1, yMax1, zMin1, zMax1, xMin2, xMax2, yMin2, yMax2, zMin2, zMax2 As Double

        shp1.Extents.GetBounds(xMin1, yMin1, zMin1, xMax1, yMax1, zMax1)
        shp2.Extents.GetBounds(xMin2, yMin2, zMin2, xMax2, yMax2, zMax2)

        If (xMin1 > xMax2 Or xMax1 < xMin2 Or yMin1 > yMax2 Or yMax1 < yMin2) Then
            overlap = False
        Else
            overlap = True
        End If
        Return overlap
    End Function

    Private Sub ExportByMask()
        Dim sourcefile, maskfile, resultfile As String
        Dim mask As New MapWinGIS.Shape
        Dim exporter As New MapWinGIS.Shape
        Dim sourceSF As New MapWinGIS.Shapefile
        Dim maskSF As New MapWinGIS.Shapefile
        Dim success As Boolean = True

        sourcefile = getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
        maskfile = getPathByName(cmbxClipWith.Items(cmbxClipWith.SelectedIndex))
        resultfile = txtbxOutFile.Text

        sourceSF.Open(sourcefile)
        maskSF.Open(maskfile)

        Dim resultSF As New MapWinGIS.Shapefile
        'delete file if it already exists
        DeleteShapefile(resultfile)
        'create the result file
        resultSF.CreateNew(resultfile, sourceSF.ShapefileType)

        Try
            resultSF.Projection = sourceSF.Projection
        Catch e As Exception
            mapwinutility.logger.dbg("DEBUG: " + e.ToString())
        End Try

        mask = maskSF.Shape(line)
        Dim rslt As Boolean = False
        If sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POINT Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTZ Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTM Then
            rslt = MapWinGeoProc.Selection.SelectPointsWithPolygon(sourceSF, mask, resultSF)
        ElseIf sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ Then
            rslt = MapWinGeoProc.Selection.SelectPolygonsWithPolygon(sourceSF, mask, resultSF)
        ElseIf sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or sourceSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ Then
            rslt = MapWinGeoProc.Selection.SelectLinesWithPolygon(sourceSF, mask, resultSF)
        End If

        If rslt Then
            If resultSF.NumShapes > 0 Then
                Dim ID As New MapWinGIS.Field
                ID.Name = "MWShapeID"
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                Dim fieldIndex As Integer
                fieldIndex = 0
                resultSF.EditInsertField(ID, fieldIndex)
                'add id values to .dbf table
                Dim numIDs As Integer
                numIDs = resultSF.NumShapes
                For i As Integer = 0 To numIDs - 1
                    resultSF.EditCellValue(0, i, i)
                Next
                resultSF.StopEditingShapes()
                resultSF.SaveAs(resultfile)
                resultSF.Close()
                If chkbxAddClip.Checked Then
                    AddMap(resultfile, 2)
                End If
            Else
                success = False
                MapWinGeoProc.Error.SetErrorMsg("No valid shapes created.")
            End If
        Else
            success = False
        End If


        maskSF.Close()
        sourceSF.Close()

        If success Then
            Me.Close()
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            System.Windows.Forms.MessageBox.Show(errorMsg, "Export Error")
            Debug.WriteLine("ErrorMsg: " + errorMsg)
            Me.Close()
        End If
    End Sub
    '4/6/06 - Angela Hillier - added for browsing to output file, will check if file is already in use before allowing user to continue
    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim enableOK As Boolean
        enableOK = True
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        'Chris M -- SAVE dialog, not open dialog
        'Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Dim fdiagOpen As New System.Windows.Forms.SaveFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1
        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                txtbxOutFile.Text = strPath
            Else
                MapWinUtility.Logger.Msg("This layer already exists. Please remove it or select a different file to save to.")
                enableOK = False
                btnOK.Enabled = False
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If
    End Sub


    Private Sub frmExportByMask_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
