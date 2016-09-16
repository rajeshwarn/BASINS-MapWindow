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
Public Class frmClipPolyWLine
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
    Friend WithEvents btnSelectPoly As System.Windows.Forms.Button
    Friend WithEvents cmbxClipWith As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseClipWith As System.Windows.Forms.Button
    Friend WithEvents btnSelectLine As System.Windows.Forms.Button
    Friend WithEvents chkbxFastClip As System.Windows.Forms.CheckBox
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents chkbxAddClip As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClipPolyWLine))
        Me.lblToClip = New System.Windows.Forms.Label
        Me.cmbxToClip = New System.Windows.Forms.ComboBox
        Me.btnBrowseToClip = New System.Windows.Forms.Button
        Me.lblClipWith = New System.Windows.Forms.Label
        Me.btnSelectPoly = New System.Windows.Forms.Button
        Me.cmbxClipWith = New System.Windows.Forms.ComboBox
        Me.btnBrowseClipWith = New System.Windows.Forms.Button
        Me.btnSelectLine = New System.Windows.Forms.Button
        Me.chkbxFastClip = New System.Windows.Forms.CheckBox
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.chkbxAddClip = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowseToOut = New System.Windows.Forms.Button
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
        'btnSelectPoly
        '
        Me.btnSelectPoly.AccessibleDescription = Nothing
        Me.btnSelectPoly.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelectPoly, "btnSelectPoly")
        Me.btnSelectPoly.BackgroundImage = Nothing
        Me.btnSelectPoly.Font = Nothing
        Me.btnSelectPoly.Name = "btnSelectPoly"
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
        'chkbxFastClip
        '
        Me.chkbxFastClip.AccessibleDescription = Nothing
        Me.chkbxFastClip.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxFastClip, "chkbxFastClip")
        Me.chkbxFastClip.BackgroundImage = Nothing
        Me.chkbxFastClip.Font = Nothing
        Me.chkbxFastClip.Name = "chkbxFastClip"
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
        'stsBar
        '
        Me.stsBar.AccessibleDescription = Nothing
        Me.stsBar.AccessibleName = Nothing
        resources.ApplyResources(Me.stsBar, "stsBar")
        Me.stsBar.BackgroundImage = Nothing
        Me.stsBar.Font = Nothing
        Me.stsBar.Name = "stsBar"
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.ForeColor = System.Drawing.Color.Firebrick
        Me.Label1.Name = "Label1"
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
        'frmClipPolyWLine
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkbxAddClip)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Controls.Add(Me.chkbxFastClip)
        Me.Controls.Add(Me.btnSelectLine)
        Me.Controls.Add(Me.btnBrowseClipWith)
        Me.Controls.Add(Me.cmbxClipWith)
        Me.Controls.Add(Me.btnSelectPoly)
        Me.Controls.Add(Me.lblClipWith)
        Me.Controls.Add(Me.btnBrowseToClip)
        Me.Controls.Add(Me.cmbxToClip)
        Me.Controls.Add(Me.lblToClip)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmClipPolyWLine"
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
    Private lineArray As ArrayList
    Private polyArray As ArrayList

    Public Sub Initialize()
        SelPolyCount = 0
        SelLineCount = 0
        lineArray = New ArrayList(0)
        polyArray = New ArrayList(0)
        stsBar.Text = "Select the polygon to clip and line to clip with."
        PopulatePolyWithLineClip()
        setDefaultColoring()
        chkbxFastClip.Enabled = True
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim okToStart As Boolean
        okToStart = False
        setDefaultColoring()

        Try
            If (cmbxToClip.Items.Count > 0) Then
                okToStart = True
            Else
                stsBar.Text = "Please select a polygon shapefile to clip."
                lblToClip.ForeColor = Drawing.Color.Red
                cmbxToClip.Focus()
                okToStart = False
            End If

            If (okToStart) Then
                If (polyArray.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a polygon."
                    btnSelectPoly.ForeColor = Drawing.Color.Red
                    btnSelectPoly.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If (cmbxClipWith.Items.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a line shapefile to clip with."
                    lblClipWith.ForeColor = Drawing.Color.Red
                    cmbxClipWith.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If (lineArray.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a line."
                    btnSelectLine.ForeColor = Drawing.Color.Red
                    btnSelectLine.Focus()
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
            stsBar.Text = "Clipping Polygon"
            ClipPolyWLine()
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
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxToClip.Items.Add(newlayer.Name)
                ElseIf newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Then
                    cmbxClipWith.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxToClip.Items.Count > 0) Then
            btnSelectPoly.Enabled = True
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
        btnSelectPoly.ForeColor = Drawing.Color.Black
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
            stsBar.Text = "0 polygons selected. Click 'Select Polygon' to select."
            btnSelectPoly.Enabled = True
            lastToClip = cmbxToClip.SelectedItem().ToString()
        Else
            btnSelectPoly.Enabled = False
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
            stsBar.Text = "0 lines selected. Click 'Select Line' to select."
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
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ _
                Then
                    AddMap(strPath, 0)
                    PopulatePolyWithLineClip()
                    lastToClip = getNameByPath(strPath)
                    cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
                Else
                    mapwinutility.logger.msg("Please select a POLYGON shapefile.")
                End If
                sf.Close()
            Else
                MapWinUtility.Logger.Msg("That layer already exists. Please select it from the drop down list.")
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
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ _
                Then
                    AddMap(strPath, 1)
                    PopulatePolyWithLineClip()
                    lastClipWith = getNameByPath(strPath)
                    cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
                Else
                    MapWinUtility.Logger.Msg("Please select a LINE shapefile.")
                End If
                sf.Close()
            Else
                MapWinUtility.Logger.Msg("That layer already exists. Please select it from the drop down list.")
            End If
        End If

        If (lastToClip <> "") Then
            cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    'get the polygons selected by the user
    Private Sub btnSelectPoly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectPoly.Click
        Dim i As Integer
        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectPoly.Text = "Select Polygon"
            SelPolyCount = g_MW.View.SelectedShapes.NumSelected
            If SelPolyCount = 0 Then
                stsBar.Text = "0 polygons selected. Click the 'Select Polygon' button to select."
            ElseIf SelPolyCount = 1 Then
                stsBar.Text = SelPolyCount.ToString + " polygon selected."
                'fill array of polygon indices
                polyArray = New ArrayList(1)
                polyArray.Add(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
            Else
                polyArray = New ArrayList(SelPolyCount)
                For i = 0 To SelPolyCount - 1
                    polyArray.Add(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                Next
                stsBar.Text = SelPolyCount.ToString + " polygons selected."
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectPoly.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnSelectLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLine.Click
        Dim i As Integer
        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectLine.Text = "Select Line"
            SelLineCount = g_MW.View.SelectedShapes.NumSelected
            If SelLineCount = 0 Then
                stsBar.Text = "0 lines selected. Click the 'Select Line' button to select."
            ElseIf SelLineCount = 1 Then
                lineArray = New ArrayList(1)
                lineArray.Add(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
                stsBar.Text = SelLineCount.ToString + " line selected."
            Else
                lineArray = New ArrayList(SelLineCount)
                For i = 0 To SelLineCount - 1
                    lineArray.Add(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                Next
                stsBar.Text = SelLineCount.ToString + " lines selected."
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
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

    Private Sub ClipPolyWLine()
        Dim polyfile, linefile, resultfile As String
        Dim polygon As New MapWinGIS.Shape
        Dim line As New MapWinGIS.Shape
        Dim polySF As New MapWinGIS.Shapefile
        Dim lineSF As New MapWinGIS.Shapefile
        Dim success As Boolean = True
        Dim i As Integer
        Dim numPolygons As Integer
        Dim numLines As Integer
        Dim fastClip As Boolean

        polyfile = getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
        linefile = getPathByName(cmbxClipWith.Items(cmbxClipWith.SelectedIndex))
        resultfile = txtbxOutFile.Text
        fastClip = chkbxFastClip.Checked

        numPolygons = polyArray.Count
        numLines = lineArray.Count
        lineSF.Open(linefile)
        polySF.Open(polyfile)

        Dim shpType As New MapWinGIS.ShpfileType
        shpType = polySF.ShapefileType
        Dim resultSF As New MapWinGIS.Shapefile
        'delete file if it already exists
        DeleteShapefile(resultfile)
        'create the result file
        resultSF.CreateNew(resultfile, shpType)

        If numPolygons = 1 And numLines = 1 Then
            polygon = polySF.Shape(polyArray.Item(0))
            line = lineSF.Shape(lineArray.Item(0))
            If MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(polygon, line, resultSF, fastClip) Then
                If resultSF.NumShapes > 0 Then
                    Dim ID As New MapWinGIS.Field
                    ID.Name = "ID"
                    ID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                    Dim fieldIndex As Integer
                    fieldIndex = 0
                    resultSF.EditInsertField(ID, fieldIndex)
                    'add id values to .dbf table
                    Dim numIDs As Integer
                    numIDs = resultSF.NumShapes
                    For i = 0 To numIDs - 1
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
                    If fastClip Then
                        MapWinGeoProc.Error.SetErrorMsg("No valid shapes created. Try without using 'fast clipping' if shapes were expected.")
                    Else
                        MapWinGeoProc.Error.SetErrorMsg("No valid shapes created.")
                    End If

                End If
            Else
                success = False
            End If

        Else
            stsBar.Text = "Clipping mulitple polygons and lines."

            Dim polySFPath, lineSFPath As String
            polySFPath = System.IO.Path.GetTempPath & "temp_poly_SF.shp"
            lineSFPath = System.IO.Path.GetTempPath & "temp_line_SF.shp"
            Dim polygons As New MapWinGIS.Shapefile
            polygons.CreateNew(polySFPath, shpType)
            Dim lines As New MapWinGIS.Shapefile
            Dim lineType As New MapWinGIS.ShpfileType
            lineType = lineSF.ShapefileType
            lines.CreateNew(lineSFPath, lineType)
            Dim shpIndex As Integer

            shpIndex = 0
            For i = 0 To numPolygons - 1
                polygons.EditInsertShape(polySF.Shape(polyArray.Item(i)), shpIndex)
                shpIndex += 1
            Next

            shpIndex = 0
            For i = 0 To numLines - 1
                lines.EditInsertShape(lineSF.Shape(lineArray.Item(i)), shpIndex)
                shpIndex += 1
            Next

            success = MapWinGeoProc.SpatialOperations.ClipPolygonSFWithLineSF(polygons, lines, resultSF)

            If resultSF.NumShapes > 0 Then
                success = True
                Dim ID As New MapWinGIS.Field
                ID.Name = "ID"
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                Dim fieldIndex As Integer
                fieldIndex = 0
                resultSF.EditInsertField(ID, fieldIndex)
                'add id values to .dbf table
                Dim numIDs As Integer
                numIDs = resultSF.NumShapes
                For i = 0 To numIDs - 1
                    resultSF.EditCellValue(0, i, i)
                Next
                'save result file and add to map
                resultSF.StopEditingShapes()
                resultSF.SaveAs(resultfile)
                resultSF.Close()
                If chkbxAddClip.Checked Then
                    RemoveLayer(resultfile)
                    AddMap(resultfile, 2)
                End If
            Else
                success = False
                resultSF.Close()
                DeleteShapefile(resultfile)
            End If
        End If
        polySF.Close()
        lineSF.Close()

        If success Then
            Me.Close()
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            System.Windows.Forms.MessageBox.Show(errorMsg, "Clipping Error")
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
        Dim fdiagSave As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
        fdiagSave.Filter = sf.CdlgFilter
        fdiagSave.FilterIndex = 1
        If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagSave.FileName
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
End Class
